namespace Core.Common.Lib.Api.Options.Api;

public class HealthCheckOptions
{
    public bool EnabledUI { get; set; }
    public bool IgnoreTlsErrors { get; set; }
    public string Location { get; set; } = "/health/ready";
    public string? Css { get; set; }
    public string? Logo { get; set; }
    public int MaximumHistoryCache { get; set; } = 50;
    public int PollingSeconds { get; set; } = 15;

    public bool IsValid() =>
        !string.IsNullOrWhiteSpace(Location) &&
        PollingSeconds > 0 &&
        MaximumHistoryCache > 0;
}
