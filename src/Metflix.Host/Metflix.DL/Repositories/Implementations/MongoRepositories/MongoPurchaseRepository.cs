using System;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Configurations;
using Metflix.Models.DbModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using static Dapper.SqlMapper;

namespace Metflix.DL.Repositories.Implementations.MongoRepositories
{
    public class MongoPurchaseRepository : IPurchaseRepository
    {
        private readonly IMongoCollection<Purchase> _purchaseCollection;
        private readonly ILogger<MongoPurchaseRepository> _logger;

        public MongoPurchaseRepository(IOptionsMonitor<MongoDbSettings> _mongoSettings, ILogger<MongoPurchaseRepository> logger)
        {
            var settings = MongoClientSettings.FromConnectionString(_mongoSettings.CurrentValue.ConnectionString);
            string databaseName = _mongoSettings.CurrentValue.DatabaseName;
            string collectionName = _mongoSettings.CurrentValue.CollectionPurchase;
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            var client = new MongoClient(settings);

            var database = client.GetDatabase(databaseName);
            _purchaseCollection = database.GetCollection<Purchase>(collectionName);
            _logger = logger;
        }

        public async Task AddPurchase(Purchase purchase, CancellationToken cancellationToken = default)
        {
            try
            {
                await _purchaseCollection.InsertOneAsync(purchase, cancellationToken: cancellationToken);
            }
            catch
            {
                _logger.LogError($"Error in {nameof(AddPurchase)}");
                throw;
            }         
           
        }

        public async Task<IEnumerable<Purchase>> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                return await (await _purchaseCollection.FindAsync(x => true)).ToListAsync();
            }
            catch
            {
                _logger.LogError($"Error in {nameof(GetAll)}");
                throw;
            }
            
        }

        public async Task<IEnumerable<Purchase>> GetAllByUserId(string userId,CancellationToken cancellationToken = default)
        {
            try
            {
                return await (await _purchaseCollection.FindAsync(x => x.UserId == userId)).ToListAsync();
            }
            catch
            {
                _logger.LogError($"Error in {nameof(GetAllByUserId)}");
                throw;
            }            
        }

        public async Task<Purchase?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return (await(await _purchaseCollection.FindAsync(x=>x.Id == id)).FirstOrDefaultAsync());
            }
            catch
            {
                _logger.LogError($"Error in {nameof(GetById)}");
                throw;
            }
        }

        public async Task<Purchase?> GetByIdAndUserId(Guid id, string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                return (await (await _purchaseCollection.FindAsync(x => x.Id == id && x.UserId == userId)).FirstOrDefaultAsync());
            }
            catch
            {
                _logger.LogError($"Error in {nameof(GetById)}");
                throw;
            }
        }

        public async Task<decimal> GetTotalSales(int minutes, CancellationToken cancellationToken = default)
        {
            try
            {
                return (await _purchaseCollection
               .AsQueryable()
               .Where(x => x.PurchaseDate >= DateTime.UtcNow.AddMinutes(minutes * -1))
               .Select(x => x.TotalPrice)
               .SumAsync());                          
            }
            catch
            {
                _logger.LogError($"Error in {nameof(GetTotalSales)}");
                throw;
            }            
        }

        public async Task<decimal> GetTotalSales(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return 0;
        }
    }
}
