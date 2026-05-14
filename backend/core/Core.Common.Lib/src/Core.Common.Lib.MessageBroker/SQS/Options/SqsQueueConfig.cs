namespace Core.Common.Lib.MessageBroker.SQS.Options;

public class SqsQueueConfig
{
    public string Url { get; set; } = string.Empty;
    public string DeadLetterUrl { get; set; } = string.Empty;
}
