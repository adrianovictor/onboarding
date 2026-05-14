using Core.Common.Lib.MessageBroker.Messaging.Common;
using Core.Common.Lib.MessageBroker.Messaging.Interfaces;
using Core.Common.Lib.MessageBroker.Messaging.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.Common.Lib.MessageBroker.Messaging.Workers;

/// <summary>
/// Worker base para consumo de mensagens.
/// Gerencia o ciclo de vida do consumer — polling, processamento, DLQ e logging.
/// </summary>
/// <param name="logger">Serviço de log para rastreamento das operações do worker.</param>
/// <param name="messageBroker">
/// Broker responsável pelas operações de recebimento, confirmação e rejeição de mensagens.
/// </param>
/// <param name="workerConfigiguration">
/// Configurações das filas — nomes, DLQ e intervalo de polling.
/// Mapeado a partir da seção <c>WorkerConfiguration</c> do <c>appsettings.json</c>.
/// </param>
/// <param name="messageServiceFactory">
/// Factory responsável por selecionar e instanciar o <see cref="IMessageService{TMessage}"/>
/// adequado para cada tipo de mensagem recebida.
/// </param>
public abstract class BaseWorker<T> : BackgroundService
    where T: IMessage
{
    private readonly IMessageBroker _messageBroker;
    private readonly WorkerConfiguration _workerConfigiguration;
    //private readonly IMessageServiceFactory _messageServiceFactory;
    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    protected virtual string QueueKey => GetType().Name;
    protected virtual string DeadLetterQueueKey => GetType().Name;


    protected BaseWorker(
        ILogger logger, 
        IServiceProvider serviceProvider, 
        IOptions<WorkerConfiguration> workerConfig, 
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = scopeFactory;
        _workerConfigiguration = workerConfig.Value;

        _messageBroker = serviceProvider.GetRequiredKeyedService<IMessageBroker>(QueueKey);
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker iniciado, monitorando a fila {QueueKey}...", QueueKey);

        var queue = _workerConfigiguration.Queues[QueueKey];

        while (!stoppingToken.IsCancellationRequested)
        {
            var message = await _messageBroker.ReceiveMessageAsync(queue.Name);
            if (message is not null)
            {
                using var scope = _serviceScopeFactory.CreateScope();

                try
                {
                    await ProcessMessageAsync(message, scope.ServiceProvider, stoppingToken);
                }
                catch (Exception ex)
                {
                    await HandleErrorAsync(ex, message, queue.DeadLetterName, stoppingToken);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(queue.PollingSeconds), stoppingToken);
        }
    }

    /// <summary>
    /// Processa a mensagem recebida da fila.
    /// Pode ser sobrescrito para adicionar lógica específica antes ou após o processmanto.
    /// </summary>
    /// <param name="message">Mensagme recebida da fila SQS</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    protected virtual async Task ProcessMessageAsync(MessageWrapper message, IServiceProvider scopeProvider, CancellationToken cancellationToken)
    {
        var mensagem = MessageDeserializer.Deserialize<T>(message.Body);
        var service = scopeProvider.GetRequiredService<IMessageService<T>>();

        await service.ProcessAsync(mensagem, cancellationToken);
        await _messageBroker.AckMessageAsync(message);
    }

    /// <summary>
    /// Trata erros ocorridos durante o processamento - envia para o DLQ e remove da fila.
    /// Pode ser sobrescrito para adicionar lógica específica para tratamento de erros.
    /// </summary>
    /// <param name="ex">Exeção ocorrida durante o processamento</param>
    /// <param name="message">Mensagem que falhou no processamento</param>
    /// <param name="dlq">URL da fila de mensagens mortas (DLQ) para onde
    /// a mensagem será enviada.</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    protected virtual async Task HandleErrorAsync(Exception ex, MessageWrapper message, string dlq, CancellationToken cancellationToken)
    {
        _logger.LogError(ex, "[{Worker}] Erro ao processar mensagem. Enviando para a DLQ. {Dlq}", GetType().Name, dlq);

        await _messageBroker.NackMessageAsync(message);
    }
}
