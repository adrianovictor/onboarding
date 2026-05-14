namespace Core.Common.Lib.MessageBroker.Messaging.Options;

public class WorkerConfiguration
{
    public const string SectionName = "WorkerConfiguration";
    public BrokerConfiguration Brokers { get; set; } = new();
    public Dictionary< string, WorkerQueueConfiguration> Queues { get; set; } = [];
}
