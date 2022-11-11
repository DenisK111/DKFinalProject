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
        private readonly IUserMovieRepository _userMovieRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly TransformBlock<PurchaseInfoData, Purchase> _recordPurchaseBlock;
        private readonly IMapper _mapper;
        private readonly ITempPurchaseDataRepository _tempPurchaseRepository;

        public PurchaseInfoDataflow(IUserMovieRepository userMovieRepository, IPurchaseRepository purchaseRepository, IMapper mapper, ITempPurchaseDataRepository tempPurchaseRepository)
        {           
            _userMovieRepository = userMovieRepository;
            _purchaseRepository = purchaseRepository;
            _mapper = mapper;
            _tempPurchaseRepository = tempPurchaseRepository;

            var options = new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = 4,
                BoundedCapacity = 4
            };

            _recordPurchaseBlock = new TransformBlock<PurchaseInfoData,Purchase>(RecordPurchase, options);            
            var writeToTempPurchaseDataBlock = new ActionBlock<Purchase>(WriteToTempPurchaseData, options);

            var linkOptions = new DataflowLinkOptions()
            {
                PropagateCompletion = true,
            };

            _recordPurchaseBlock.LinkTo(writeToTempPurchaseDataBlock, linkOptions);
                        
        }

        private async Task WriteToTempPurchaseData(Purchase purchase)
        {
            var purchaseAsByteArray = MessagePackSerializer.Serialize(purchase);
            await _tempPurchaseRepository.SetOrUpdateEntryAsync(purchase.UserId, purchaseAsByteArray);
        }

        public void Dispose()
        {

        }

        public async Task ProcessData(PurchaseInfoData data, CancellationToken cancellationToken = default)
        {
            await _recordPurchaseBlock.SendAsync(data, cancellationToken);
        }
       

        private async Task<Purchase> RecordPurchase(PurchaseInfoData data)
        {                        
            var userMovies = new List<UserMovie>();

            foreach (var movie in data.MovieInfoData)
            {
                var UserMovie = new UserMovie()
                {
                    MovieId = movie.Id,
                    UserId = data.UserId,
                    DaysFor = data.Days,
                };

                userMovies.Add(UserMovie);               
            }

            var userMovieIds = await _userMovieRepository.DecreaseAvailableQuantityAndAddUserMoviesTransaction(userMovies);

            var fullPurchaseInfoData = _mapper.Map<FullPurchaseInfoData>(data);
            fullPurchaseInfoData.UserMovieIds = userMovieIds;          

            var purchase = new Purchase()
            {
                Days = data.Days,
                UserMovieIds = fullPurchaseInfoData.UserMovieIds,
                Movies = _mapper.Map<IEnumerable<MovieRecord>>(data.MovieInfoData),
                UserId = data.UserId,
                TotalPrice = data.MovieInfoData.Select(x => x.PricePerDay).Sum() * data.Days,
                PurchaseDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(data.Days),
                LastChanged = DateTime.UtcNow
            };

            try
            {
                await _purchaseRepository.AddPurchase(purchase);
            }
            catch
            {
                await _userMovieRepository.ReverseDecreaseAvailableQuantityAndAddUserMoviesTransaction(userMovieIds, data.MovieInfoData.Select(x => x.Id));
                    throw;
            }
            
            return purchase;               
        }
    }
}
