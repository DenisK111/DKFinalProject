using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace Metflix.Host.HealthChecks
{
    public class RedisHealthCheck : IHealthCheck
    {
        private readonly IConnectionMultiplexer _connection;

        public RedisHealthCheck(IConnectionMultiplexer connection)
        {
            _connection = connection;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                foreach (var endPoint in _connection.GetEndPoints(configuredOnly: true))
                {
                    var server = _connection.GetServer(endPoint);
                    await _connection.GetDatabase().PingAsync();
                    await server.PingAsync();

                }

                return HealthCheckResult.Healthy("Redis connection is Ok.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(ex.Message);
            }
        }
    }
}
