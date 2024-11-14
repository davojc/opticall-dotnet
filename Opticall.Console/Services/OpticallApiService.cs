using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Opticall.Console.Commands;
using Opticall.Console.Config;
using Opticall.Console.Luxafor;
using Opticall.Console.Requests;

namespace Opticall.Console.Services;

public class OpticallApiService : BackgroundService
{
    private readonly ISettingsProvider _settingsProvider;
    private readonly IHost _webHost;

    public OpticallApiService(ILogger<OpticallApiService> logger, ISettingsProvider settingsProvider, ILuxaforDeviceManager luxaforDeviceManager)
    {
        var onCommand = new OnCommand();
        var offCommand = new OffCommand();
        var patternCommand = new PatternCommand();
        var waveCommand = new WaveCommand();
        var fadeCommand = new FadeCommand();
        var strobeCommand = new StrobeCommand();

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

                                var on = JsonSerializer.Deserialize<OnRequest>(requestBody);
                                var onBytes = on.ToBytes();
                                var cmd = onCommand.CreateCommand(led, onBytes);
                                luxaforDeviceManager.Run(cmd);
                            });

                            endpoints.MapPost($"/led/{ledName}/off/", async context =>
                            {
                                var off = new OffRequest();
                                var offBytes = off.ToBytes();
                                var cmd = offCommand.CreateCommand(led, offBytes);
                                luxaforDeviceManager.Run(cmd);
                            });

                            endpoints.MapPost($"/led/{ledName}/strobe/", async context =>
                            {
                                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

                                var strobe = JsonSerializer.Deserialize<StrobeRequest>(requestBody);
                                var strobeBytes = strobe.ToBytes();
                                var cmd = strobeCommand.CreateCommand(led, strobeBytes);
                                luxaforDeviceManager.Run(cmd);
                            });

                            endpoints.MapPost($"/led/{ledName}/fade/", async context =>
                            {
                                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

                                var fade = JsonSerializer.Deserialize<FadeRequest>(requestBody);
                                var fadeBytes = fade.ToBytes();
                                var cmd = fadeCommand.CreateCommand(led, fadeBytes);
                                luxaforDeviceManager.Run(cmd);
                            });
                        }

                        var options = new JsonSerializerOptions
                        {
                            Converters = { new JsonStringEnumConverter() }
                        };

                        endpoints.MapPost($"/led/pattern/", async context =>
                        {
                            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

                            var pattern = JsonSerializer.Deserialize<PatternRequest>(requestBody, options);
                            var patternBytes = pattern.ToBytes();
                            var cmd = patternCommand.CreateCommand(patternBytes);
                            luxaforDeviceManager.RunDirect(cmd);
                        });

                        endpoints.MapPost($"/led/wave/", async context =>
                        {
                            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

                            var wave = JsonSerializer.Deserialize<WaveRequest>(requestBody, options);
                            var waveBytes = wave.ToBytes();
                            var cmd = waveCommand.CreateCommand(waveBytes);
                            luxaforDeviceManager.RunDirect(cmd);
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


