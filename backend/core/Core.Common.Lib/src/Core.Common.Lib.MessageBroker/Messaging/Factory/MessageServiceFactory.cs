using Core.Common.Lib.MessageBroker.Messaging.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Common.Lib.MessageBroker.Messaging.Factory;

public class MessageServiceFactory(IServiceProvider serviceProvider) : IMessageServiceFactory
{
    public IMessageService<TMessage> Create<TMessage>(TMessage message) where TMessage : IMessage
    {
        var service = serviceProvider.GetService<IMessageService<TMessage>>() ??
            throw new InvalidOperationException($"Nenhum serviço registrado para o tipo {typeof(TMessage).Name}.");

        return service;
    }
}
