using Core.Common.Lib.MessageBroker.Messaging.Common;
using Core.Common.Lib.MessageBroker.Messaging.Interfaces;
using Core.Common.Lib.MessageBroker.SQS.Interfaces;
using Core.Common.Lib.MessageBroker.SQS.Options;
using Microsoft.Extensions.Options;

namespace Core.Common.Lib.MessageBroker.Messaging.Adapters;

public class SqsMessageBrokerAdapter(ISqsMessageBroker sqsBroker, IOptions<SqsMessagingConfig> config) : IMessageBroker
{
    public async Task AckMessageAsync(MessageWrapper message)
    {
        if (message.BrokerMetadata is not SqsMetadata metadata)
            throw new InvalidOperationException(
                $"BrokerMetadata inváldo para SQS, Tipo recebido: {message.BrokerMetadata?.GetType().Name}.");

        await sqsBroker.DeleteMessageAsync(metadata.QueueUrl, metadata.ReceiptHandle);        
    }

    public async Task NackMessageAsync(MessageWrapper message)
    {
        if (message.BrokerMetadata is not SqsMetadata metadata)
            throw new InvalidOperationException(
                $"BrokerMetadata inváldo para SQS, Tipo recebido: {message.BrokerMetadata?.GetType().Name}.");

        await sqsBroker.SendMessageAsync(metadata.DeadLetterUrl, message.Body);
        await sqsBroker.DeleteMessageAsync(metadata.QueueUrl, metadata.ReceiptHandle);
    }

    public async Task<MessageWrapper?> ReceiveMessageAsync(string source)
    {
        var msg = await sqsBroker.ReceiveMessageAsync(source);
        if (msg is null)
            return null;

        var dlq = config.Value.Queues.Values.FirstOrDefault(_ => _.Url == source)?.DeadLetterUrl ?? string.Empty;

        return new MessageWrapper
        {
            Body = msg.Body,
            BrokerMetadata = new SqsMetadata
            {
                QueueUrl = source,
                ReceiptHandle = msg.ReceiptHandle,
                DeadLetterUrl    = dlq
            }
        };
    }

    public Task SendMessageAsync(string detination, string body) =>
        sqsBroker.SendMessageAsync(detination, body);
}
