using System.Diagnostics.CodeAnalysis;
using Core.Common.Lib.MessageBroker.SQS.Interfaces;
using Core.Common.Lib.MessageBroker.SQS.Options;
using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Common.Lib.MessageBroker.SQS.Extensions;

[ExcludeFromCodeCoverage]
public static class SqsMessageBrokerExtensions
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

            services.AddSingleton<ISqsMessageBroker>(sp =>
            {
                var credentials = new BasicAWSCredentials(accessKey, secretKey);

                if (environment.Equals("Development", StringComparison.OrdinalIgnoreCase))
                {
                    var sqsClient = new AmazonSQSClient(
                        credentials, new AmazonSQSConfig
                        {
                            ServiceURL = "http://localhost:4566", // LocalStack SQS endpoint
                            AuthenticationRegion = region
                        });

                    return new SqsMessageBroker(sqsClient);
                }
                                
                var client = new AmazonSQSClient(
                    credentials, 
                    Amazon.RegionEndpoint.GetBySystemName(region));
                
                return new SqsMessageBroker(client);
            });
            
            services.Configure<SqsMessagingConfig>(configuration.GetSection("AwsConfig:SQS"));

            return services;
        }
    }
}
