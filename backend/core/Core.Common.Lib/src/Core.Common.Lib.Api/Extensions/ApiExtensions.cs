using System;
using Core.Common.Lib.Api.Contexts;
using Core.Common.Lib.Api.Middlewares;
using Core.Common.Lib.Api.Options.Api;
using Core.Common.Lib.Api.Options.Jwt;
using Core.Common.Lib.Application.Context;
using Core.Common.Lib.Application.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Common.Lib.Api.Extensions;

public static class ApiExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApiExtensions(IConfiguration configuration)
        {
            services.AddOptions<ApiOptions>()
                .Bind(configuration.GetSection("ApiConfiguration"))
                .Validate(config => config.IsValid(), "ApiConfiguration: Name, Title, Description and Version are required.")
                .ValidateOnStart();

            services.AddOptions<HealthCheckOptions>()
                .Bind(configuration.GetSection("HealthCheck"))
                .Validate(config => config.IsValid(), "HealthCheck: Endpoint and Interval are required.")
                .ValidateOnStart();

            services.AddOptions<List<ExternalApiOptions>>()
                .Bind(configuration.GetSection("ExternalApis"))
                .Validate(config => config.All(api => api.IsValid()), "Each ExternalApi must have Name and Url defined.")
                .ValidateOnStart();

            // services.AddOptions<JwtOptions>()
            //     .Bind(configuration.GetSection("Jws"))
            //     .Validate(config => config.IsValid(), "Jws: Key and Algorithm are required.")
            //     .ValidateOnStart();

            services.AddHttpContextAccessor();
            services.AddScoped<GlobalExceptionMiddleware>();
            //services.AddScoped<JwtMiddleware>();
            services.AddScoped<IRequestContext, RequestContext>();
            services.AddScoped(typeof(ILogService<>), typeof(LogService<>));

            return services;
        }
    }
}
