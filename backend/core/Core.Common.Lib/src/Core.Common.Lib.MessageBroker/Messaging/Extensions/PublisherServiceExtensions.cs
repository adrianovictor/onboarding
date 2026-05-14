using Core.Common.Lib.MessageBroker.Messaging.Interfaces;
using Core.Common.Lib.MessageBroker.Messaging.Publishing;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Common.Lib.MessageBroker.Messaging.Extensions;

public static class PublisherServiceExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddMessagePublisher<T>() where T : IMessage
        {
            services.AddSingleton<IMessagePublisher<T>, MessagePublisher<T>>();
            
            return services;
        }
    }
}
