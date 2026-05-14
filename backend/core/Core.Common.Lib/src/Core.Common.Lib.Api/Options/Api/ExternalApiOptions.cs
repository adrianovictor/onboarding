namespace Core.Common.Lib.Api.Options.Api;

public class ExternalApiOptions
{
    public string Name { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string? HealthCheckEndpoint { get; set; }

    public string ResolveHealthUrl() =>
        !string.IsNullOrWhiteSpace(HealthCheckEndpoint) ? HealthCheckEndpoint : BaseUrl;
    
    public bool HasHealthCheck() =>
        !string.IsNullOrWhiteSpace(HealthCheckEndpoint);

    public bool IsValid() =>
        !string.IsNullOrWhiteSpace(Name) &&
        !string.IsNullOrWhiteSpace(BaseUrl);
}
