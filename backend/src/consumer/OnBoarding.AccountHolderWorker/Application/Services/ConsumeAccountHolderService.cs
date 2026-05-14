using Core.Common.Lib.MessageBroker.Messaging.Interfaces;
using OnBoarding.AccountHolderWorker.Domain.Messages;

namespace OnBoarding.AccountHolderWorker.Application.Services;

public class ConsumeAccountHolderService(ILogger<ConsumeAccountHolderService> logger) : IMessageService<AccountHolderMessage>
{
    public Task ProcessAsync(AccountHolderMessage message, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processando mensagem de titular de conta: {Message}", message);

        return Task.CompletedTask;
    }
}
