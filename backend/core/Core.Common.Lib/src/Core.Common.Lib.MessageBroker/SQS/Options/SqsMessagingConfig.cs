namespace Core.Common.Lib.MessageBroker.SQS.Options;

public class SqsMessagingConfig
{
    public Dictionary<string, SqsQueueConfig> Queues { get; set; } = [];
    public int MaxNumberOfMessages { get; set; } = 10;
    public int WaitTimeSeconds { get; set; } = 20;
    public int VisibilityTimeout { get; set; } = 30;
}
