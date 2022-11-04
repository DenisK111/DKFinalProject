using System;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Configurations;
using Metflix.Models.DbModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Metflix.DL.Repositories.Implementations.MongoRepositories
{
    public class MongoPurchaseRepository : IPurchaseRepository
    {
        private readonly IMongoCollection<Purchase> _purchaseCollection;
        private readonly ILogger<MongoPurchaseRepository> _logger;

        public MongoPurchaseRepository(IOptionsMonitor<MongoDbSettings> _mongoSettings, ILogger<MongoPurchaseRepository> logger)
        {
            string connectionString = _mongoSettings.CurrentValue.ConnectionString;
            string databaseName = _mongoSettings.CurrentValue.DatabaseName;
            string collectionName = _mongoSettings.CurrentValue.CollectionPurchase;

            var client = new MongoClient(connectionString);

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

        public async Task<IEnumerable<Purchase>> GetAllForUser(Guid userId,CancellationToken cancellationToken = default)
        {
            try
            {
                return await (await _purchaseCollection.FindAsync(x => x.UserId == userId)).ToListAsync();
            }
            catch
            {
                _logger.LogError($"Error in {nameof(GetAllForUser)}");
                throw;
            }            
        }

        public async Task<decimal> GetTotalSales(int minutes, CancellationToken cancellationToken = default)
        {
            try
            {
                 return (await (await _purchaseCollection
                .FindAsync(x => x.PurchaseDate >= DateTime.UtcNow.AddMinutes(minutes * -1)))
                .ToListAsync()).Sum(x=>x.TotalPrice);
            }
            catch
            {
                _logger.LogError($"Error in {nameof(GetAllForUser)}");
                throw;
            }            
        }
    }
}
