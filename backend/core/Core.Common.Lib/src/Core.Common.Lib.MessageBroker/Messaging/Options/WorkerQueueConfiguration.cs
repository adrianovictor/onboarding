using Core.Common.Lib.MessageBroker.Messaging.Enums;

namespace Core.Common.Lib.MessageBroker.Messaging.Options;

public class WorkerQueueConfiguration
{
    public BrokerType BrokerType { get; set; } = BrokerType.SNS;
    public string Name { get; set; } = string.Empty;
    public string DeadLetterName { get; set; } = string.Empty;
    public int PollingSeconds { get; set; } = 15;
}
