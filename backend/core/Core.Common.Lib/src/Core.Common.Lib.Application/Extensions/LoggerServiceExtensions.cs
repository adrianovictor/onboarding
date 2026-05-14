using Core.Common.Lib.Application.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Transport;

namespace Core.Common.Lib.Application.Extensions;

public static class LoggerServiceExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddLogService(IConfiguration configuration, Action<LogOptions>? configure = null)
        {
            var options = new LogOptions();

            configuration.GetSection("ApiConfiguration").Bind(options);
            configuration.GetSection("Logging:Options").Bind(options);
            options.Graylog = configuration.GetSection("Graylog").Get<GraylogOptions>();
            configure?.Invoke(options);

            // Registra as options no container para uso pelo LogService
            services.Configure<LogOptions>(o =>
            {
                o.ServiceName   = options.ServiceName;
                o.Environment   = options.Environment;
                o.IncludeScopes = options.IncludeScopes;
                o.Graylog       = options.Graylog;
            });

            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment",  options.Environment)
                .Enrich.WithProperty("Application",  options.ServiceName)
                .Enrich.WithProperty("MachineName",  Environment.MachineName)
                .Enrich.WithExceptionDetails();

            if (options.Graylog?.IsConfigured is true)
            {
                var transportType = Enum.TryParse<TransportType>(
                        options.Graylog.TransportType, ignoreCase: true, out var parsed)
                    ? parsed
                    : TransportType.Udp;

                loggerConfig.WriteTo.Graylog(new GraylogSinkOptions
                {
                    HostnameOrAddress    = options.Graylog.Host,
                    Port                 = options.Graylog.Port,
                    TransportType        = transportType,
                    MinimumLogEventLevel = LogEventLevel.Information
                });
            }

            loggerConfig.WriteTo.Console(
                outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{Application}] {Message:lj}{NewLine}{Exception}");

            Log.Logger = loggerConfig.CreateLogger();

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog(dispose: true);
            });

            return services;
        }
    }
}