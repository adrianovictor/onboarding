using Core.Common.Lib.MessageBroker.RabbitMQ.Common;

namespace Core.Common.Lib.MessageBroker.RabbitMQ.Interfaces;

/// <summary>
/// Define o contrato para operações de envio, recebimento, confirmação (ack) e 
/// rejeição (nack) de mensagens utilizando RabbitMQ como message broker.
/// </summary>
public interface IRabbitMqMessageBroker
{
    /// <summary>
    /// Publica uma mensagem em formato JSON para um destino específico (fila ou exchange) no RabbitMQ.
    /// </summary>
    /// <param name="destination">Nome da fila de destino</param>
    /// <param name="body">Conteúdo da mensagem em formato JSON</param>
    /// <returns></returns>
    Task SendMessageAsync(string destination, string body);

    /// <summary>
    /// Recebe uma mensagem da fila especificada, retornando um wrapper contendo o corpo da mensagem e a tag de entrega.
    /// </summary>
    /// <param name="source">Nome da fila de origem</param>
    /// <returns>
    /// <see cref="RabbitMqMessageWrapper"/> com o corpo e delivery tag ou <c>null</c> se não houver mensagens disponíveis
    /// </returns>
    Task<RabbitMqMessageWrapper?> ReceiveMessageAsync(string source);

    /// <summary>
    /// Confirma o recebimento de uma mensagem utilizando a tag de entrega, removendo-a da fila.
    /// </summary>
    /// <param name="deliveryTag">Tag de entrega da mensagem</param>
    /// <returns></returns>
    Task AckMessageAsync(ulong deliveryTag);

    /// <summary>
    /// Rejeita o recebimento de uma mensagem utilizando a tag de entrega, podendo opcionalmente reencaminhá-la para a fila.
    /// </summary>
    /// <param name="deliveryTag">Tag de entrega da mensagem</param>
    /// <param name="requeue">Indica se a mensagem deve ser reencaminhada para a fila</param>
    /// <returns></returns>
    Task NackMessageAsync(ulong deliveryTag, bool requeue = false);
}
