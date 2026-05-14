using Core.Common.Lib.MessageBroker.SNS.Interfaces;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace Core.Common.Lib.MessageBroker.SNS;

/// <summary>
/// Implementação do <see cref="ISnsMessageBroker"/> usando o Amazon Simple Notification Service (SNS) para criar tópicos, 
/// publicar mensagens e gerenciar assinaturas. Esta classe encapsula a lógica de interação com o SNS, 
/// permitindo que os consumidores usem uma interface simples para trabalhar com tópicos e mensagens no SNS. 
/// O construtor recebe uma instância do IAmazonSimpleNotificationService, que é usada para realizar as 
/// operações de criação de tópicos, publicação de mensagens e gerenciamento de assinaturas. 
/// Cada método é assíncrono, permitindo que as operações sejam realizadas de forma eficiente sem bloquear a 
/// execução do programa.
/// </summary>
/// <param name="snsClient">Instância do IAmazonSimpleNotificationService</param>
public class SnsMessageBroker(IAmazonSimpleNotificationService snsClient) : ISnsMessageBroker
{
    private readonly IAmazonSimpleNotificationService _snsClient = snsClient;

    /// <summary>
    /// Cria um tópico no SNS e retorna o ARN do tópico criado.
    /// </summary>
    /// <param name="topicName">Nome do tópico a ser criado</param>
    /// <returns>ARN do tópico criado</returns>
    public async Task<string> CreateTopicAsync(string topicName)
    {
        var response = await _snsClient.CreateTopicAsync(new CreateTopicRequest
        {
            Name = topicName
        });

        return response.TopicArn;
    }

    /// <summary>
    /// Publica uma mensagem em um tópico SNS especificado pelo ARN. O parâmetro "subject" é opcional e pode ser usado para fornecer um título ou assunto para a mensagem, o que pode ser útil para organizar e identificar mensagens no SNS.
    /// </summary>
    /// <param name="topicArn">ARN do tópico em que a mensagem será publicada</param>
    /// <param name="message">Mensagem a ser publicada</param>
    /// <param name="subject">Título ou assunto da mensagem (opcional)</param>
    public async Task PublishMessageAsync(string topicArn, string message, string? subject = null)
    {
        await _snsClient.PublishAsync(new PublishRequest
        {
            TopicArn = topicArn,
            Message = message,
            Subject = subject
        });
    }

    /// <summary>
    /// Inscreve um endpoint (como um endereço de e-mail, número de telefone ou URL) em um tópico SNS especificado pelo ARN. O parâmetro "protocol" define o protocolo de comunicação para o endpoint (por exemplo, "email", "sms", "http", etc.). O SNS enviará mensagens para o endpoint inscrito sempre que uma mensagem for publicada no tópico correspondente.
    /// </summary>
    /// <param name="topicArn">ARN do tópico ao qual o endpoint será inscrito</param>
    /// <param name="protocol">Protocolo de comunicação para o endpoint</param>
    /// <param name="endpoint">Endereço do endpoint a ser inscrito</param>
    public async Task SubscribeAsync(string topicArn, string protocol, string endpoint)
    {
        await _snsClient.SubscribeAsync(new SubscribeRequest
        {
            TopicArn = topicArn,
            Protocol = protocol,
            Endpoint = endpoint
        });
    }
}
