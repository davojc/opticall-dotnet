using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Opticall.Console.Config;
using Opticall.Console.Luxafor;

namespace Opticall.Console.Services;

public class OpticallApiService : BackgroundService
{
    private readonly IHost _webHost;

    public OpticallApiService(ILogger<OpticallApiService> logger, ISettingsProvider settingsProvider, ILuxaforDeviceManager luxaforDeviceManager)
    {
        var builder = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    services.AddSingleton<ISettingsProvider>(settingsProvider);
                    services.AddSingleton<ILuxaforDeviceManager>(luxaforDeviceManager);
                    services.AddControllers();
                    services.AddEndpointsApiExplorer();
                    services.AddSwaggerGen();
                    services.AddRouting(options => options.LowercaseUrls = true);
                });

                webBuilder.Configure(app =>
                {
                    app.UseMiddleware<ErrorHandlingMiddleware>();
                    app.UseSwagger();
                    app.UseSwaggerUI();
                    app.UseRouting();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                });
            });
        
         _webHost = builder.Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _webHost.RunAsync(stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _webHost.StopAsync(cancellationToken);
    }
}


