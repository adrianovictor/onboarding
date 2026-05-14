namespace Core.Common.Lib.MessageBroker.RabbitMQ.Options;

/// <summary>
/// Configuração de uma fila RabbitMQ, contendo propriedades específicas para a criação e gerenciamento de filas, como nome, durabilidade, exclusividade e auto-delete.
/// </summary>
public class RabbitMqQueueConfiguration
{
    /// <summary>
    /// Nome da fila principal
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// NOme da fila de mensagens mortas (dead letter queue) associada a esta fila, onde mensagens rejeitadas ou expiradas serão encaminhadas
    /// </summary>
    public string DeadLetterName { get; set; } = string.Empty;
}
