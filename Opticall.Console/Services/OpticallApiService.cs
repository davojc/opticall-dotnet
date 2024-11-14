using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Opticall.Console.Command.Commands;
using Opticall.Console.Config;
using Opticall.Console.Luxafor;

namespace Opticall.Console.Services;

public class OpticallApiService : BackgroundService
{
    private readonly ISettingsProvider _settingsProvider;
    private readonly IHost _webHost;

    public OpticallApiService(ILogger<OpticallApiService> logger, ISettingsProvider settingsProvider, ILuxaforDeviceManager luxaforDeviceManager)
    {
        _settingsProvider = settingsProvider;
        _webHost = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.Configure(app =>
                {
                    app.UseMiddleware<ErrorHandlingMiddleware>();
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

                                var on = JsonSerializer.Deserialize<OnCommand>(requestBody);
                                on.Led = led;
                                luxaforDeviceManager.Run(on);
                            });

                            endpoints.MapPost($"/led/{ledName}/off/", async context =>
                            {
                                var off = new OffCommand
                                {
                                    Led = led
                                };
                                luxaforDeviceManager.Run(off);
                            });

                            endpoints.MapPost($"/led/{ledName}/strobe/", async context =>
                            {
                                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

                                var strobe = JsonSerializer.Deserialize<StrobeCommand>(requestBody);
                                strobe.Led = led;
                                luxaforDeviceManager.Run(strobe);
                            });

                            endpoints.MapPost($"/led/{ledName}/fade/", async context =>
                            {
                                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

                                var fade = JsonSerializer.Deserialize<FadeCommand>(requestBody);
                                fade.Led = led;
                                luxaforDeviceManager.Run(fade);
                            });
                        }

                        var options = new JsonSerializerOptions
                        {
                            Converters = { new JsonStringEnumConverter() }
                        };

                        endpoints.MapPost($"/led/pattern/", async context =>
                        {
                            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

                            var pattern = JsonSerializer.Deserialize<PatternCommand>(requestBody, options);
                            luxaforDeviceManager.Run(pattern);
                        });

                        endpoints.MapPost($"/led/wave/", async context =>
                        {
                            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

                            var wave = JsonSerializer.Deserialize<WaveCommand>(requestBody, options);
                            luxaforDeviceManager.Run(wave);
                        });
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


