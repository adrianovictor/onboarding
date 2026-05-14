using Core.Common.Lib.Api.HealthChecks;
using Core.Common.Lib.Api.Options.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Common.Lib.Api.Extensions;

public static class HealthCheckExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddHealthChecksExtensions(IConfiguration configuration)
        {
            var apiConfig = configuration.GetSection("ApiConfiguration").Get<ApiOptions>();
            var healthConfig = configuration.GetSection("HealthCheck").Get<HealthCheckOptions>();
            
            if (apiConfig is null)
                throw new InvalidOperationException("ApiConfiguration section is missing or invalid in appsettings.");
            if (healthConfig is null)
                throw new InvalidOperationException("HealthCheck section is missing or invalid in appsettings.");

            var builder = services.AddHealthChecks();

            builder.AddCheck<DatabaseHealthCheck>("Database", tags: ["ready", "Infra", apiConfig.Title]);

            var externalApis = configuration.GetSection("ExternalApis").Get<List<ExternalApiOptions>>() ?? [];

            if (healthConfig.EnabledUI)
            {
                services.AddHealthChecksUI(setup =>
                {
                    setup.SetEvaluationTimeInSeconds(healthConfig.PollingSeconds);
                    setup.MaximumHistoryEntriesPerEndpoint(healthConfig.MaximumHistoryCache);
                    setup.AddHealthCheckEndpoint(apiConfig.Title, healthConfig.Location);
                    setup.SetHeaderText(apiConfig.Title);

                    if (healthConfig.IgnoreTlsErrors)
                    {
                        setup.UseApiEndpointHttpMessageHandler(_ =>
                        {
                            return new HttpClientHandler
                            {
                                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                            };
                        });
                    }
                }).AddInMemoryStorage();
            }

            return services;
        }
    }
}
