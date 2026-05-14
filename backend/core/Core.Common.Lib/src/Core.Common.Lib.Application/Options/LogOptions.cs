namespace Core.Common.Lib.Application.Options;

public sealed class LogOptions
{
    public string ServiceName { get; set; } = "UnknownService";
    public string Environment { get; set; } = "Production";
    public bool IncludeScopes { get; set; } = true;

    public GraylogOptions? Graylog { get; set; }
}
