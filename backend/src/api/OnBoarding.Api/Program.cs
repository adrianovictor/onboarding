using Core.Common.Lib.Api.Extensions;
using Core.Common.Lib.Application.Extensions;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using AssemblyReference = OnBoarding.Application.AssemblyReference;
using AspNetHealthCheckOptions = Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddApiExtensions(builder.Configuration);
builder.Services.AddLogService(builder.Configuration);
builder.Services.AddSwaggerExtensions();

builder.Services.AddMediatorExtensions(typeof(AssemblyReference).Assembly!);
builder.Services.AddValidatorsFromAssemblyContaining<AssemblyReference>();

builder.Services.AddCors(opts =>
    opts.AddPolicy("Angular", p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod()));

builder.Services.AddHealthChecks();

builder.Services.AddHealthChecksUI(setup =>
{
    setup.SetEvaluationTimeInSeconds(30);
    setup.AddHealthCheckEndpoint("OnBoarding API", "/health");
}).AddInMemoryStorage();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseSwaggerExtensions();
app.UseHttpsRedirection();
app.UseCors("Angular");
app.MapControllers();

app.UseHealthChecks("/health", new AspNetHealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [HealthStatus.Healthy]   = StatusCodes.Status200OK,
        [HealthStatus.Degraded]  = StatusCodes.Status503ServiceUnavailable,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.UseHealthChecksUI(setup =>
{
    setup.UIPath = "/dashboard";
});

app.Run();