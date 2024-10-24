using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Opticall.Console;

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<OpticallService>();
    }).UseWindowsService();

hostBuilder.Build().Run();