using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using AutoMapper;
using Metflix.BL.Dataflow.Contracts;
using Metflix.DL.Repositories.Contracts;
using Metflix.Kafka.Producers;
using Metflix.Models.Configurations.KafkaSettings.Producers;
using Metflix.Models.KafkaModels;

namespace Metflix.BL.Dataflow.Implementations
{
    public class PurchaseUserInputDataFlow : IPurchaseUserInputDataflow
    {
        private readonly IMovieRepository _movieRepository;
        private readonly TransformBlock<PurchaseUserInputData, PurchaseInfoData> _getPurchaseInfoBlock;
        private readonly IMapper _mapper;
        private readonly GenericProducer<string, PurchaseInfoData, KafkaPurchaseDataProducerSettings> _producer;

        public PurchaseUserInputDataFlow(IMovieRepository movieRepository, IMapper mapper, GenericProducer<string, PurchaseInfoData, KafkaPurchaseDataProducerSettings> producer)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _producer = producer;

            var options = new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = 4,
                BoundedCapacity = 4
            };

            _getPurchaseInfoBlock = new TransformBlock<PurchaseUserInputData, PurchaseInfoData>(GetPurchaseData, options);
            var produceBlock = new ActionBlock<PurchaseInfoData>(ProduceToTopic, options);

            var linkOptions = new DataflowLinkOptions()
            {
                PropagateCompletion = true,
            };

            _getPurchaseInfoBlock.LinkTo(produceBlock, linkOptions);            
        }

        private async Task ProduceToTopic(PurchaseInfoData purchaseInfoData)
        {
            Console.WriteLine(purchaseInfoData);
            await _producer.ProduceAsync(purchaseInfoData.GetKey(), purchaseInfoData);            
        }

        public async Task ProcessData(PurchaseUserInputData data, CancellationToken cancellationToken = default)
        {
            await _getPurchaseInfoBlock.SendAsync(data, cancellationToken);
        }

        private async Task<PurchaseInfoData> GetPurchaseData(PurchaseUserInputData userInput)
        {
            var movieInfos = new List<MovieInfoData>();
            foreach (var movieId in userInput.MovieIds)
            {
                var movie = await _movieRepository.GetById(movieId);
                movieInfos.Add(_mapper.Map<MovieInfoData>(movie));
            }

            return new PurchaseInfoData()
            {
                UserId = userInput.Id,
                Days = userInput.Days,
                MovieInfoData = movieInfos
            };
        }

        public void Dispose()
        {
            _producer.Dispose();
        }
    }
}
