using Core.Common.Lib.MessageBroker.Messaging.Options;
using Core.Common.Lib.MessageBroker.Messaging.Workers;
using Microsoft.Extensions.Options;
using OnBoarding.AccountHolderWorker.Domain.Messages;

namespace OnBoarding.AccountHolderWorker.Workers;

public class ConsumeAccountHolderWorker(
    ILogger<ConsumeAccountHolderWorker> logService,
    IServiceProvider serviceProvider,
    IOptions<WorkerConfiguration> workerConfig,
    IServiceScopeFactory scopeFactory) : BaseWorker<AccountHolderMessage>(logService, serviceProvider, workerConfig, scopeFactory)
{
    protected override string QueueKey => nameof(ConsumeAccountHolderWorker);
}
