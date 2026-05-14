namespace Core.Common.Lib.MessageBroker.Messaging.Common;

public class SqsMetadata
{
    public string QueueUrl { get; set; } = string.Empty;
    public string ReceiptHandle { get; set; } = string.Empty;
    public string DeadLetterUrl { get; set; } = string.Empty;    
}
