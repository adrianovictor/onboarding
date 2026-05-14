using Core.Common.Lib.MessageBroker.SQS.Common;

namespace Core.Common.Lib.MessageBroker.SQS.Interfaces;

public interface ISqsMessageBroker
{
    Task SendMessageAsync(string detination, string body);
    Task<SqsMessageWrapper?> ReceiveMessageAsync(string source);
    Task DeleteMessageAsync(string source, string receiptHandle);
}
