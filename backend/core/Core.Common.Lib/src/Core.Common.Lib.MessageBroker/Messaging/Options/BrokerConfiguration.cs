namespace Core.Common.Lib.MessageBroker.Messaging.Options;

public class BrokerConfiguration
{
    public SqsBrokerConfiguration? SQS { get; set; }
    public RabbitMqBrokerConfiguration? RabbitMQ { get; set; }
}
