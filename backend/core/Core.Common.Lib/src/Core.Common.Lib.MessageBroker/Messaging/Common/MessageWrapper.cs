namespace Core.Common.Lib.MessageBroker.Messaging.Common;

public class MessageWrapper
{
    public string Body { get; set; } = string.Empty;
    public object? BrokerMetadata { get; set; } = null;
}
