using System.Data.SqlClient;
using Metflix.Models.DbModels.Configurations;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Metflix.Host.HealthChecks
{
    public class SqlHealthCheck : IHealthCheck
    {
        private readonly IOptionsMonitor<ConnectionStrings> _configuration;

        public SqlHealthCheck(IOptionsMonitor<ConnectionStrings> configuration)
        {
            _configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            await using (var connection = new SqlConnection(_configuration.CurrentValue.SqlConnection))
            {
                try
                {
                    await connection.OpenAsync(cancellationToken);
                }
                catch (SqlException e)
                {

                    return HealthCheckResult.Unhealthy(e.Message);
                }
            }

            return HealthCheckResult.Healthy("Sql connection is OK");
        }
    }
}
