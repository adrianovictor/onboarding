namespace Core.Common.Lib.MessageBroker.Messaging.Interfaces;

/// <summary>
/// Definie o contrato para processamento de mensagens de um determinado tipo. Implementações dessa interface devem conter a lógica necessária para lidar com as mensagens recebidas, incluindo validação, transformação e execução de ações específicas com base no conteúdo da mensagem.
/// </summary>
/// <typeparam name="TMessage">
/// O tipo de mensagem a ser processada. Deve implementar <see cref="IMessage"/>.
/// </typeparam>
/// <example>
/// Implementação para processar mensagens de um tipo específico:
/// <code>
/// public class OrderMessageService : IMessageService<OrderMessage>
/// {
///   public async Task ProcessAsync(OrderMessage message, CancellationToken cancellationToken)
///  {
///   // Lógica para processar a mensagem de pedido
///  }
/// }
/// </code>
/// </example>
public interface IMessageService<TMessage>
    where TMessage : IMessage
{
        /// <summary>
    /// Processa a mensagem recebida aplicando a lógica especifica do dominio.
    /// </summary>
    /// <param name="message">Mensagem a ser processada</param>
    /// <param name="cancellationToken">Token de cancelamento da operação</param>
    /// <returns>Uma <see cref="Task"/> que representa a operação assincrona.</returns>
    /// <exception cref="MessageProcessingException">
    /// Dispara quando ocorre um erro durante o processamento da mensagem
    /// </exception>
    Task ProcessAsync(TMessage message, CancellationToken cancellationToken);
}
