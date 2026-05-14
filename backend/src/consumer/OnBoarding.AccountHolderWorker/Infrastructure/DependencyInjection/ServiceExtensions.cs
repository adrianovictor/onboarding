using Core.Common.Lib.MessageBroker.Messaging.Extensions;
using Core.Common.Lib.MessageBroker.Messaging.Interfaces;
using OnBoarding.AccountHolderWorker.Application.Services;
using OnBoarding.AccountHolderWorker.Domain.Messages;
using OnBoarding.AccountHolderWorker.Workers;

namespace OnBoarding.AccountHolderWorker.Infrastructure.DependencyInjection;

public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddConfigurationServices(IConfiguration configuration)
        {
            services.AddWorkerServices(configuration);
            services.AddHostedServices();
            services.AddFeatures();

            return services;
        }

        private IServiceCollection AddHostedServices()
        {
            services.AddHostedService<ConsumeAccountHolderWorker>();

            return services;
        }

        private IServiceCollection AddFeatures()
        {
            services.AddScoped<IMessageService<AccountHolderMessage>, ConsumeAccountHolderService>();

            return services;
        }
    }
}