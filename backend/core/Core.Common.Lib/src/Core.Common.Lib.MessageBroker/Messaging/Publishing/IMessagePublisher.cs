using Core.Common.Lib.MessageBroker.Messaging.Interfaces;

namespace Core.Common.Lib.MessageBroker.Messaging.Publishing;

public interface IMessagePublisher<T> where T : IMessage
{
    Task PublishAsync(T message, CancellationToken cancellationToken = default);
}
