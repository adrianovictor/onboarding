using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Core.Common.Lib.Api.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Common.Lib.Api.Extensions;

public static class SwaggerExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddSwaggerExtensions()
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type =>
                {
                    static string Format(Type t)
                    {
                        var asm = t.Assembly.GetName().Name?.Split('.').Skip(2).FirstOrDefault() ?? "Asm";

                        var name = t.Name.Replace("+", "_");

                        if (t.IsGenericType)
                        {
                            var genericArgs = string.Join("_", t.GetGenericArguments().Select(Format));
                            name = $"{name}_{genericArgs}";
                        }

                        return $"{asm}_{name}";
                    }

                    return Format(type);
                });
            });

            services.ConfigureOptions<SwaggerVersioningConfiguration>();

            return services;
        }
    }
}

public static class ApplicationBuilderExtensions
{
    extension(IApplicationBuilder app)
    {
        public IApplicationBuilder UseSwaggerExtensions()
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions.Reverse())
                {
                    c.SwaggerEndpoint($"{description.GroupName}/swagger.json", $"API {description.GroupName.ToUpper()}");
                }

                c.RoutePrefix = "swagger";
            });

            return app;
        }
    }
}
