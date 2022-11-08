using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metflix.DL.Repositories.Contracts;
using Metflix.Models.Configurations;
using Metflix.Models.DbModels;
using Metflix.Models.KafkaModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Metflix.DL.Repositories.Implementations.MongoRepositories
{
    public class MongoInventoryLogRepository : IInventoryLogRepository
    {
        private readonly IMongoCollection<InventoryLog> _purchaseCollection;
        private readonly ILogger<MongoInventoryLogRepository> _logger;

        public MongoInventoryLogRepository(IOptionsMonitor<MongoDbSettings> _mongoSettings, ILogger<MongoInventoryLogRepository> logger)
        {
            var settings = MongoClientSettings.FromConnectionString(_mongoSettings.CurrentValue.ConnectionString);
            string databaseName = _mongoSettings.CurrentValue.DatabaseName;
            string collectionName = _mongoSettings.CurrentValue.CollectionInventoryLogData;
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            var client = new MongoClient(settings);

            var database = client.GetDatabase(databaseName);
            _purchaseCollection = database.GetCollection<InventoryLog>(collectionName);
            _logger = logger;
        }

        public async Task AddLog(InventoryLog data, CancellationToken cancellationToken = default)
        {
            try
            {
                 await _purchaseCollection.InsertOneAsync(data, cancellationToken: cancellationToken);
               
            }
            catch
            {
                _logger.LogError($"Error in {nameof(AddLog)}");
                throw;
            }
        }

        public async Task<IEnumerable<InventoryLog>> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                return await(await _purchaseCollection.FindAsync(x => true)).ToListAsync();
            }
            catch
            {
                _logger.LogError($"Error in {nameof(GetAll)}");
                throw;
            }
        }

        public async Task<InventoryLog> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return (await(await _purchaseCollection.FindAsync(x => x.Id == id)).FirstOrDefaultAsync());
            }
            catch
            {
                _logger.LogError($"Error in {nameof(GetById)}");
                throw;
            }
        }
    }
}
