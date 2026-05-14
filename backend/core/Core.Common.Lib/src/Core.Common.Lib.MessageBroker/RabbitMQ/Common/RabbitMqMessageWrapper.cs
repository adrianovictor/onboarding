namespace Core.Common.Lib.MessageBroker.RabbitMQ.Common;

/// <summary>
/// Encasula uma mensagem recebida do RabbitMQ, contendo o corpo da 
/// mensagem e a tag de entrega, utilizada para confirmar ou rejeitar o recebimento da mensagem
/// </summary>
public class RabbitMqMessageWrapper
{
    /// <summary>
    /// Conteúdo da mensagem em format JSON
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Tag de entrega da mensagem, utilizado para confirmar o recebimento (ack) ou rejeição (nack) da mensagem
    /// </summary>
    public ulong DeliveryTag { get; set; }
}
