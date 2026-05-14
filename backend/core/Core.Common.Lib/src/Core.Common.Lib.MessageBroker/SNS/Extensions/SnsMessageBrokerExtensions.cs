using System.Diagnostics.CodeAnalysis;
using Core.Common.Lib.MessageBroker.SNS.Interfaces;
using Core.Common.Lib.MessageBroker.SNS.Options;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Common.Lib.MessageBroker.SNS.Extensions;

[ExcludeFromCodeCoverage]
public static class SnsMessageBrokerExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddSnsMessageBroker(IConfiguration configuration)
        {
            var awsConfig = configuration.GetSection("AwsConfig");
            var region = awsConfig.GetValue<string>("Region");
            var accessKey = awsConfig.GetValue<string>("Credentials:AccessKey");
            var secretKey = awsConfig.GetValue<string>("Credentials:SecretKey");

            var environment = configuration.GetValue<string>("DOTNET_ENVIRONMENT") ?? "Production";

            services.AddSingleton<ISnsMessageBroker>(sp =>
            {
                var credentials = new BasicAWSCredentials(accessKey, secretKey);

                IAmazonSimpleNotificationService snsClient;
                if (environment.Equals("Development", StringComparison.OrdinalIgnoreCase))
                {
                    snsClient = new AmazonSimpleNotificationServiceClient(
                        credentials, new AmazonSimpleNotificationServiceConfig
                        {
                            ServiceURL = "http://localhost:4566", // LocalStack SNS endpoint
                            AuthenticationRegion = region
                        });

                    return new SnsMessageBroker(snsClient);
                }
                                
                snsClient = new AmazonSimpleNotificationServiceClient(
                    credentials, 
                    Amazon.RegionEndpoint.GetBySystemName(region));
                
                return new SnsMessageBroker(snsClient);
            });
            
            services.Configure<SnsMessagingConfig>(configuration.GetSection("AwsConfig:SNS"));

            return services;
        }
    }
}
