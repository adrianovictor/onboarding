using Core.Common.Lib.MessageBroker.Messaging.Common;
using Core.Common.Lib.MessageBroker.Messaging.Interfaces;
using Core.Common.Lib.MessageBroker.Messaging.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.Common.Lib.MessageBroker.Messaging.Publishing;

public sealed class MessagePublisher<T> : IMessagePublisher<T>
    where T : IMessage
{
    private readonly IMessageBroker _broker;
    private readonly string _queueName;
    private readonly ILogger<MessagePublisher<T>> _logger;

    public MessagePublisher(
        IServiceProvider serviceProvider,
        IOptions<WorkerConfiguration> config,
        ILogger<MessagePublisher<T>> logger)
    {
        _logger = logger;

        var queueKey = typeof(T).Name; // convenção igual ao QueueKey do BaseWorker

        _broker = serviceProvider.GetRequiredKeyedService<IMessageBroker>(queueKey);
        
        if (!config.Value.Queues.TryGetValue(queueKey, out var queue))
            throw new InvalidOperationException(
                $"Fila '{queueKey}' não encontrada em WorkerConfiguration:Queues.");

        _queueName = queue.Name;
    }

    public async Task PublishAsync(T message, CancellationToken cancellationToken = default)
    {
        var body = MessageSerializer.Serialize(message);

        _logger.LogInformation("[Publisher] Enviando mensagem {Type} para {Queue}.", 
            typeof(T).Name, _queueName);

        await _broker.SendMessageAsync(_queueName, body);
    }
}
