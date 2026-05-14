using System.Data.Common;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Core.Common.Lib.Api.HealthChecks;

public class DatabaseHealthCheck(DbConnection connection) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await connection.OpenAsync(cancellationToken);
            await connection.CloseAsync();

            return HealthCheckResult.Healthy("Database connection is healthy.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database connection is unhealthy.", ex);
        }
    }
}
