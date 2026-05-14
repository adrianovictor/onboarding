using System.Reflection;
using Core.Common.Lib.Api.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Common.Lib.Api.Extensions;

public static class MediatorExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddMediatorExtensions(params Assembly[] assemblies)
        {
            if (assemblies is not { Length: > 0 })
                throw new ArgumentException(
                    "Informe ao menos um assembly para o MediatR escanear os handlers.",
                    nameof(assemblies));

            services.AddMediatR(cfg =>
            {
                foreach (var assembly in assemblies)
                    cfg.RegisterServicesFromAssembly(assembly);

                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            return services;
        }
    }
}