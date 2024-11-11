using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Opticall.Console.Commands;
using Opticall.Console.Config;
using Opticall.Console.Luxafor;
using Opticall.Console.Model;

namespace Opticall.Console.Services;

public class OpticallApiService : BackgroundService
{
    private readonly ISettingsProvider _settingsProvider;
    private readonly IHost _webHost;

    public OpticallApiService(ILogger<OpticallApiService> logger, ISettingsProvider settingsProvider, ILuxaforDeviceManager luxaforDeviceManager)
    {
        var onCommand = new OnCommand();
        var offCommand = new OffCommand();

        _settingsProvider = settingsProvider;
        _webHost = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.Configure(app =>
                {
                    app.UseRouting();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("/config/target", async context =>
                        {
                            await context.Response.WriteAsync(_settingsProvider.Target);
                        });

                        endpoints.MapGet("/config/group", async context =>
                        {
                            await context.Response.WriteAsync(_settingsProvider.Group);
                        });

                        endpoints.MapPut("/config/target", async context =>
                        {
                            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                            _settingsProvider.SetTarget(requestBody);
                            await context.Response.WriteAsync(_settingsProvider.Target);
                        });

                        endpoints.MapPut("/config/group", async context =>
                        {
                            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                            _settingsProvider.SetGroup(requestBody);
                            await context.Response.WriteAsync(_settingsProvider.Group);

                        });


                        var leds = Enum.GetValues(typeof(Led));

                        foreach (var led in leds.Cast<Led>())
                        {
                            var ledName = Enum.GetName(typeof(Led), led).ToLowerInvariant();

                            endpoints.MapPost($"/led/{ledName}/on/", async context =>
                            {
                                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

                                var on = JsonSerializer.Deserialize<On>(requestBody);
                                var onBytes = on.ToBytes(led);
                                var cmd = onCommand.CreateCommand(onBytes);
                                luxaforDeviceManager.Run(cmd);
                            });
                        }

                        foreach (var led in leds.Cast<Led>())
                        {
                            var ledName = Enum.GetName(typeof(Led), led).ToLowerInvariant();

                            endpoints.MapPost($"/led/{ledName}/off/", async context =>
                            {
                                var off = new Off();
                                var offBytes = off.ToBytes(led);
                                var cmd = offCommand.CreateCommand(offBytes);
                                luxaforDeviceManager.Run(cmd);
                            });
                        }
                    });
                });

            }).Build();
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


