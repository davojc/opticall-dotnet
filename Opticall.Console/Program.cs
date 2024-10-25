using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Opticall.Console;
using Opticall.Console.Commands;
using Opticall.Console.IO;
using Opticall.Console.Luxafor;

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables();
    })
    .ConfigureLogging((context, logging) =>
    {
        //logging.ClearProviders();

        logging.AddConfiguration(context.Configuration.GetSection("Logging"));

        logging.SetMinimumLevel(LogLevel.Warning);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<ILuxaforDeviceManager, LuxaforDeviceManager>();
        services.AddSingleton<ICommandRouter, CommandRouter>();
        services.AddSingleton<ICommandListener, OscUdpListener>();

        services.AddHostedService<OpticallService>();
    }).UseWindowsService();

hostBuilder.Build().Run();