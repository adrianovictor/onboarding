using System.Text;
using Core.Common.Lib.MessageBroker.RabbitMQ.Common;
using Core.Common.Lib.MessageBroker.RabbitMQ.Interfaces;
using RabbitMQ.Client;

namespace Core.Common.Lib.MessageBroker.RabbitMQ;

public class RabbitMqMessageBroker(IChannel channel, IConnection connection) : IRabbitMqMessageBroker, IAsyncDisposable
{
    private IChannel _channel = channel;
    private readonly IConnection _connection = connection;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private async Task<IChannel> GetChannelAsync()
    {
        if (_channel.IsOpen)
            return _channel;

        await _semaphore.WaitAsync();
        try
        {
            if (_channel.IsOpen)
                return _channel;

            _channel.Dispose();
            _channel = await _connection.CreateChannelAsync();
            return _channel;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Criar uma instância de RabbitMqMessageBroker utilizando uma conexão RabbitMQ existente, garantindo que o canal seja criado de forma assíncrona e thread-safe.
    /// </summary>
    /// <param name="connection">Conexão ativa com o RabbitMQ</param>
    /// <returns>Umma instância do <see cref="RabbitMqMessageBroker"/> pronta para ser utilizada.</returns>
    public static async Task<RabbitMqMessageBroker> CreateAsync(IConnection connection)
    {
        var channel = await connection.CreateChannelAsync();

        return new RabbitMqMessageBroker(channel, connection);        
    }

    /// <inheritdoc/>
    public async Task AckMessageAsync(ulong deliveryTag)
    {
        var channel = await GetChannelAsync();

        await channel.BasicAckAsync(deliveryTag, false);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        _channel.Dispose();
    }

    /// <inheritdoc/>
    public async Task NackMessageAsync(ulong deliveryTag, bool requeue = false)
    {
        var channel = await GetChannelAsync();

        await channel.BasicNackAsync(deliveryTag, multiple: false, requeue);
    }

    /// <inheritdoc/>
    public async Task<RabbitMqMessageWrapper?> ReceiveMessageAsync(string source)
    {
        var channel = await GetChannelAsync();
        var result = await channel.BasicGetAsync(source, autoAck: false);

        if (result == null)
            return null;

        return new RabbitMqMessageWrapper
        {
            Body = Encoding.UTF8.GetString(result.Body.ToArray()),
            DeliveryTag = result.DeliveryTag
        };
    }

    /// <inheritdoc/>
    public async Task SendMessageAsync(string destination, string body)
    {
        var channel = await GetChannelAsync();
        var bodyBytes = Encoding.UTF8.GetBytes(body);

        var properties = new BasicProperties
        {
            Persistent = true,
            ContentType = "application/json",
        };

        await channel.BasicPublishAsync(
            exchange: string.Empty, 
            routingKey: destination, 
            mandatory: false, 
            basicProperties: properties, 
            body: bodyBytes);
    }
}
