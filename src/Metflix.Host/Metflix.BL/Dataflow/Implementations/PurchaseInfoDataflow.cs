using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using AutoMapper;
using MessagePack;
using Metflix.BL.Dataflow.Contracts;

using Metflix.DL.Repositories.Contracts;
using Metflix.Models.DataflowModels;
using Metflix.Models.DbModels;
using Metflix.Models.KafkaModels;

namespace Metflix.BL.Dataflow.Implementations
{
    public class PurchaseInfoDataflow : IPurchaseInfoDataflow
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUserMovieRepository _userMovieRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly TransformBlock<PurchaseInfoData, PurchaseInfoData> _adjustInventoryBlock;
        private readonly IMapper _mapper;
        private readonly ITempPurchaseDataRepository _tempPurchaseRepository;

        public PurchaseInfoDataflow(IMovieRepository movieRepository, IUserMovieRepository userMovieRepository, IPurchaseRepository purchaseRepository, IMapper mapper, ITempPurchaseDataRepository tempPurchaseRepository)
        {
            _movieRepository = movieRepository;
            _userMovieRepository = userMovieRepository;
            _purchaseRepository = purchaseRepository;
            _mapper = mapper;
            _tempPurchaseRepository = tempPurchaseRepository;

            var options = new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = 4,
                BoundedCapacity = 4
            };

            _adjustInventoryBlock = new TransformBlock<PurchaseInfoData, PurchaseInfoData>(AdjustInventory, options);
            var writeToUserMoviesBlock = new TransformBlock<PurchaseInfoData, FullPurchaseInfoData>(WriteToUserMovies, options);
            var recordPurchaseBlock = new ActionBlock<FullPurchaseInfoData>(RecordPurchase, options);

            var linkOptions = new DataflowLinkOptions()
            {
                PropagateCompletion = true,
            };

            _adjustInventoryBlock.LinkTo(writeToUserMoviesBlock, linkOptions);
            writeToUserMoviesBlock.LinkTo(recordPurchaseBlock, linkOptions);            
        }

        public void Dispose()
        {

        }

        public async Task ProcessData(PurchaseInfoData data, CancellationToken cancellationToken = default)
        {
            await _adjustInventoryBlock.SendAsync(data, cancellationToken);
        }

        private async Task<PurchaseInfoData> AdjustInventory(PurchaseInfoData data)
        {
            foreach (var movieId in data.MovieInfoData.Select(x => x.Id))
            {
                await _movieRepository.DecreaseAvailableQuantity(movieId);
            }

            return data;
        }

        private async Task<FullPurchaseInfoData> WriteToUserMovies(PurchaseInfoData data)
        {
            var userMovieIds = new List<int>();

            foreach (var movie in data.MovieInfoData)
            {
                var UserMovie = new UserMovie()
                {
                    MovieId = movie.Id,
                    UserId = data.UserId,
                    DaysFor = data.Days,
                };

                var userMovieId = await _userMovieRepository.Add(UserMovie);
                userMovieIds.Add(userMovieId);
            }

            var fullPurchaseInfoData = _mapper.Map<FullPurchaseInfoData>(data);
            fullPurchaseInfoData.UserMovieIds = userMovieIds;
            return fullPurchaseInfoData;
        }

        private async Task RecordPurchase(FullPurchaseInfoData data)
        {
            var purchase = new Purchase()
            {
                Days = data.Days,
                UserMovieIds = data.UserMovieIds,
                Movies = _mapper.Map<IEnumerable<MovieRecord>>(data.MovieInfoData),
                UserId = data.UserId,
                TotalPrice = data.MovieInfoData.Select(x => x.PricePerDay).Sum() * data.Days,
                PurchaseDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(data.Days),
                LastChanged = DateTime.UtcNow
            };

            await _purchaseRepository.AddPurchase(purchase);
            var purchaseAsByteArray = MessagePackSerializer.Serialize(purchase);
            await _tempPurchaseRepository.SetOrUpdateEntryAsync(purchase.UserId,purchaseAsByteArray);          
        }
    }
}
