namespace Core.Common.Lib.MessageBroker.SQS.Common;

public class SqsMessageWrapper
{
    public string Body { get; set; } = string.Empty;
    public string ReceiptHandle { get; set; } = string.Empty;
}
