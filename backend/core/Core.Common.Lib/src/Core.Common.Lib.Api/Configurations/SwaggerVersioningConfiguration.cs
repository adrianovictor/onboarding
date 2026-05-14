using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Core.Common.Lib.Api.Options.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Core.Common.Lib.Api.Configurations;

public class SwaggerVersioningConfiguration(
    IApiVersionDescriptionProvider provider, 
    IOptions<ApiOptions> apiOptions) : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider = provider;
    private readonly ApiOptions _apiOptions = apiOptions.Value;

    public void Configure(SwaggerGenOptions options)
    {
        var xmlFiles = new HashSet<string>();
        var baseDirectory = AppContext.BaseDirectory;

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(baseDirectory, xmlFile);
        
        if (File.Exists(xmlPath))
            xmlFiles.Add(xmlPath);

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                var assemblyName = assembly.GetName().Name;
                var xmlFileName = $"{assemblyName}.xml";
                var xmlFilePath = Path.Combine(baseDirectory, xmlFileName);

                if (File.Exists(xmlFilePath))
                    xmlFiles.Add(xmlFilePath);
            }
            catch
            {
                // Ignorar assemblies que não podem ser carregados
            }
        }

        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName, new OpenApiInfo
                {
                    Title = $"{_apiOptions.Title} {description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                    Description = _apiOptions.Description
                });
        }

        options.ResolveConflictingActions(apiDescription => apiDescription.FirstOrDefault());

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Type = SecuritySchemeType.ApiKey,
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecuritySchemeReference("Bearer", doc),
                new List<string>()
            }
        });

        foreach (var file in xmlFiles)
            options.IncludeXmlComments(file, includeControllerXmlComments: true);
    }
}
