
using OnBoarding.AccountHolderWorker.Infrastructure.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddConfigurationServices(builder.Configuration);

var host = builder.Build();
host.Run();
