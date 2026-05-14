namespace Core.Common.Lib.Application.Options;

public sealed class GraylogOptions
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 12201;

    /// <summary>Udp (padrão), Tcp ou Http.</summary>
    public string TransportType { get; set; } = "Udp";

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(Host) && Port > 0;
}
