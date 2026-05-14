using System.Net.Http.Json;
using Core.Common.Lib.Api.Options.Api;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Core.Common.Lib.Api.HealthChecks;

public class ExternalApiHealthCheck(IHttpClientFactory httpClientFactory, IOptions<List<ExternalApiOptions>> externalApiOptions) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var results = new Dictionary<string, object>();
        var unhealthyApis = new HashSet<string>();
        var degradedApis = new HashSet<string>();

        foreach (var api in externalApiOptions.Value)
        {
            try
            {
                var client = httpClientFactory.CreateClient(api.Name);
                client.Timeout = TimeSpan.FromSeconds(5);

                var response = await client.GetAsync(api.ResolveHealthUrl(), cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    results[api.Name] = $"Indisponível - Status Code: {response.StatusCode}";
                    unhealthyApis.Add(api.Name);
                    continue;
                }

                if (api.HasHealthCheck())
                {
                    var content = await response.Content
                        .ReadFromJsonAsync<ExternalHealthResponse>(
                            cancellationToken: cancellationToken);

                    var status = content?.Status?.ToLowerInvariant() ?? "unknown";

                    results[api.Name] = status switch
                    {
                        "healthy" => "Disponível",
                        "degraded" => "Degradado",
                        "unhealthy" => "Indisponível",
                        _ => $"Desconecido - Status: {status}"
                    };

                    if (status == "degraded")
                        degradedApis.Add(api.Name);

                    if (status is not ("healthy" or "degraded"))
                        unhealthyApis.Add(api.Name);
                } else
                {
                    results[api.Name] = $"Disponível - Status Code: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                results[api.Name] = $"Error: {ex.Message}";
                unhealthyApis.Add(api.Name);
            }
        }

        if (unhealthyApis.Count > 0)
        {
            return HealthCheckResult.Unhealthy(
                description: $"Alguns APIs externas estão indisponíveis: {string.Join(", ", unhealthyApis)}.", null, results);
        }

        if (degradedApis.Count > 0)
        {
            return HealthCheckResult.Degraded(
                description: $"Algumas APIs externas estão com desempenho degradado: {string.Join(", ", degradedApis)}.", null, results);
        }

        return HealthCheckResult.Healthy(
            description: "Todas as APIs externas estão disponíveis.",
            data: results);
    }
}
