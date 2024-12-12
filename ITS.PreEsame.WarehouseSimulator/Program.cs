using ITS.PreEsame.WarehouseSimulator;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("ServiceBus");

builder.Services.AddLogging();
builder.Services.AddHostedService<Worker>(serviceProvider =>
{
    var logger = serviceProvider.GetRequiredService<ILogger<Worker>>();
    return new Worker(connectionString ?? "", logger);
});

var host = builder.Build();
host.Run();
