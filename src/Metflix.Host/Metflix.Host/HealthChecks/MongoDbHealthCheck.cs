using Metflix.Models.Configurations;
using Metflix.Models.DbModels.Configurations;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Metflix.Host.HealthChecks
{
    public class MongoDbHealthCheck : IHealthCheck
    {
        private IMongoDatabase _db { get; set; }
        public MongoClient _mongoClient { get; set; }

        public MongoDbHealthCheck(IOptionsMonitor<MongoDbSettings> configuration)
        {
            _mongoClient = new MongoClient(configuration.CurrentValue.ConnectionString);
            _db = _mongoClient.GetDatabase(configuration.CurrentValue.DatabaseName);
        }              

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await _db.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
            }

            catch (Exception e)
            {
                return HealthCheckResult.Unhealthy(e.Message);
            }

            return HealthCheckResult.Healthy("Mongo connection is OK");
        }
    }
}
