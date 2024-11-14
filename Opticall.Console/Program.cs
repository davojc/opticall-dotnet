using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Opticall.Console;
using Opticall.Console.Commands;
using Opticall.Console.Config;
using Opticall.Console.IO;
using Opticall.Console.Luxafor;
using Opticall.Console.Services;

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("logging.json", optional: true, reloadOnChange: true);
    })
    .ConfigureLogging((context, logging) =>
    {
        logging.AddConfiguration(context.Configuration.GetSection("Logging"));

        logging.SetMinimumLevel(LogLevel.Warning);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<ILuxaforDeviceManager, LuxaforDeviceManager>();
        services.AddSingleton<ICommandRouter, CommandRouter>();
        services.AddSingleton<ICommandListener, OscUdpListener>();
        services.AddSingleton<ISettingsProvider, SettingsProvider>();

        services.AddHostedService<OpticallService>();
        services.AddHostedService<OpticallApiService>();
    }).UseWindowsService();

hostBuilder.Build().Run();