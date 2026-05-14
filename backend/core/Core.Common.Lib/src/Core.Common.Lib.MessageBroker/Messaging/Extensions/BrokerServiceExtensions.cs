using Core.Common.Lib.MessageBroker.Messaging.Adapters;
using Core.Common.Lib.MessageBroker.Messaging.Enums;
using Core.Common.Lib.MessageBroker.Messaging.Interfaces;
using Core.Common.Lib.MessageBroker.Messaging.Options;
using Core.Common.Lib.MessageBroker.RabbitMQ;
using Core.Common.Lib.MessageBroker.RabbitMQ.Interfaces;
using Core.Common.Lib.MessageBroker.SQS;
using Core.Common.Lib.MessageBroker.SQS.Interfaces;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Core.Common.Lib.MessageBroker.Messaging.Extensions;

public static class BrokerServiceExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddWorkerServices(IConfiguration configuration)
        {
            var workerConfig = configuration.GetSection(WorkerConfiguration.SectionName).Get<WorkerConfiguration>() ?? new();

            services.Configure<WorkerConfiguration>(configuration.GetSection(WorkerConfiguration.SectionName));

            var brokerTypes = workerConfig.Queues.Values.Select(_ => _.BrokerType).Distinct();

            foreach (var brokerType in brokerTypes)
            {
                switch (brokerType)
                {
                    case BrokerType.RabbitMQ:
                        services.AddRabbitMqBrokerService(workerConfig);
                        break;
                    case BrokerType.SQS:
                        services.AddSqsBrokerService(workerConfig);
                        break;
                    default:
                        throw new InvalidOperationException($"Tipo de broker não suportado: {brokerType}");
                }
            }

            return services;
        }

        private IServiceCollection AddSqsBrokerService(WorkerConfiguration workerConfig)
        {
            var sqsConfig = workerConfig.Brokers.SQS ?? 
                throw new InvalidOperationException("SQS não configurado em WorkerConfiguration:Brokers:SQS.");

            services.AddSingleton<ISqsMessageBroker>(sp =>
            {
                var credentials = new BasicAWSCredentials(
                    sqsConfig.Credentials.AccessKey, 
                    sqsConfig.Credentials.SecretKey);

                var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
                if (environment.Equals("Development", StringComparison.OrdinalIgnoreCase))
                {
                    var sqsClientConfig = new AmazonSQSConfig
                    {
                        ServiceURL = sqsConfig.Url,
                        AuthenticationRegion = sqsConfig.Region
                    };

                    return new SqsMessageBroker(new AmazonSQSClient(credentials, sqsClientConfig));
                }

                return new SqsMessageBroker(new AmazonSQSClient(credentials, RegionEndpoint.GetBySystemName(sqsConfig.Region)));
            });

            services.AddSingleton<SqsMessageBrokerAdapter>();
            foreach (var queue in workerConfig.Queues.Where(_ => _.Value.BrokerType == BrokerType.SQS))
                services.AddKeyedSingleton<IMessageBroker, SqsMessageBrokerAdapter>(queue.Key);                

            return services;
        }

        private IServiceCollection AddRabbitMqBrokerService(WorkerConfiguration workerConfig)
        {
            var rabbitConfig = workerConfig.Brokers.RabbitMQ ?? 
                throw new InvalidOperationException("RabbitMQ não configurado em WorkerConfiguration:Brokers:RabbitMQ.");

            services.AddSingleton<IConnection>(sp =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = rabbitConfig.Host,
                    Port = rabbitConfig.Port,
                    UserName = rabbitConfig.Username,
                    Password = rabbitConfig.Password
                };

                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            });

            services.AddSingleton<IRabbitMqMessageBroker>(sp =>
            {
                var connection = sp.GetRequiredService<IConnection>();
                return RabbitMqMessageBroker.CreateAsync(connection).GetAwaiter().GetResult();
            });

            services.AddSingleton<RabbitMqMessageBrokerAdapter>();
            foreach (var queue in workerConfig.Queues.Where(_ => _.Value.BrokerType == BrokerType.RabbitMQ))
                services.AddKeyedSingleton<IMessageBroker, RabbitMqMessageBrokerAdapter>(queue.Key);

            return services;
        }
    }
}
