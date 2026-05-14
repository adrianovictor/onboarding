namespace Core.Common.Lib.MessageBroker.RabbitMQ.Options;

/// <summary>
/// Configurações do sistema de mensageria do RabbitMQ
/// mapeado a partir da seção <c>RabbitConfig</c> do <c>appsettings.json</c>.
/// </summary>
public class RabbitMqMessagingConfiguration
{
    public const string SectionName = "RabbitConfig";

    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 5672;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = "/";

    public Dictionary<string, RabbitMqQueueConfiguration> Queues { get; set; } = [];
}
