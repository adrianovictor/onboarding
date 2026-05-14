using Core.Common.Lib.MessageBroker.Messaging.Common;
using Core.Common.Lib.MessageBroker.Messaging.Interfaces;
using Core.Common.Lib.MessageBroker.RabbitMQ.Interfaces;

namespace Core.Common.Lib.MessageBroker.Messaging.Adapters;

public class RabbitMqMessageBrokerAdapter(IRabbitMqMessageBroker rabbitBroker) : IMessageBroker
{
    public Task AckMessageAsync(MessageWrapper message)
    {
        if (message.BrokerMetadata is not RabbitMqMetadata metadata)
            throw new InvalidOperationException(
                $"BrokerMetadata inváldo para RabbitMQ, Tipo recebido: {message.BrokerMetadata?.GetType().Name}.");

        return rabbitBroker.AckMessageAsync(metadata.DeliveryTag);
    }

    public Task NackMessageAsync(MessageWrapper message)
    {
        if (message.BrokerMetadata is not RabbitMqMetadata metadata)
            throw new InvalidOperationException(
                $"BrokerMetadata inváldo para RabbitMQ, Tipo recebido: {message.BrokerMetadata?.GetType().Name}."); 

        return rabbitBroker.NackMessageAsync(metadata.DeliveryTag);       
    }

    public async Task<MessageWrapper?> ReceiveMessageAsync(string source)
    {
        var msg = await rabbitBroker.ReceiveMessageAsync(source);
        if (msg is null)
            return null;

        return new MessageWrapper
        {
            Body = msg.Body,
            BrokerMetadata = new RabbitMqMetadata
            {
                DeliveryTag = msg.DeliveryTag
            }
        };
    }

    public Task SendMessageAsync(string detination, string body) =>
        rabbitBroker.SendMessageAsync(detination, body);
}
