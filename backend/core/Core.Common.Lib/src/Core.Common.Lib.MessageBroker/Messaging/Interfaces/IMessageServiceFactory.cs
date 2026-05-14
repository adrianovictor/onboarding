namespace Core.Common.Lib.MessageBroker.Messaging.Interfaces;

/// <summary>
/// Definie o contrato para a factory responsável por selecionar e instanciar
/// o <see cref="IMessageService{TMessage}"/> adequado para cada tipo de mensagem.
/// Implementa o padrão Strategy - cada tipo de mensagem tem sua própria estratégia de processamento
/// </summary>
/// <example>
/// Uso na pipe de processamento
/// <code>
/// var mensagem - MessageDeserializer.Deserializer&alt;EnviarEmailMessage&gt;(body);
/// await factory;Create(mensagem).ProcessAsync(mensagem, cancellationToken)
/// </code>
/// </example>
public interface IMessageServiceFactory
{
    /// <summary>
    /// Instancia o serviço de processamento adequado para o tipo de mensagem informada
    /// </summary>
    /// <typeparam name="TMessage">Tipo da mensagem. Deve implementar <see cref="IMessage"/></typeparam>
    /// <param name="message">Mensagem para qual o serviço será instanciado</param>
    /// <returns><see cref="IMessageService{TMessage}"/></returns>
    /// <exception cref="InvalidOperationException">
    /// Dispara quando não existe um serviço registrado para o tipo de mensagem informada.
    /// </exception>    
    IMessageService<TMessage> Create<TMessage>(TMessage message)
        where TMessage : IMessage;
}
