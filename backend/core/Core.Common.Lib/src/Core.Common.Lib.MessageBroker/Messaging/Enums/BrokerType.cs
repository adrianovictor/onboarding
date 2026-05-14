namespace Core.Common.Lib.MessageBroker.Messaging.Enums;

/// <summary>
/// Define os tipos de brokers de mensagens suportados
/// </summary>
public enum BrokerType
{
    /// <summary>
    /// SNS service
    /// </summary>
    SNS = 1,

    /// <summary>
    /// SQS service
    /// </summary>
    SQS = 2,

    /// <summary>
    /// RabbitMQ service
    /// </summary>
    RabbitMQ = 3
};
