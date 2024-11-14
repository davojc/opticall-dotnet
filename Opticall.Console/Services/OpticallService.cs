using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Opticall.Console.Command;
using Opticall.Console.Command.Commands;
using Opticall.Console.Config;
using Opticall.Console.Luxafor;

namespace Opticall.Console.Services;

public class OpticallService(
    ILogger<OpticallService> logger,
    ILuxaforDeviceManager luxaforDeviceManager,
    ICommandRouter commandRouter,
    ICommandListener commandListener,
    ISettingsProvider settingsProvider)
    : BackgroundService
{
    private readonly ICommandDeserializer _serializer = new CommandOscDeserializer();

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        luxaforDeviceManager.Initialise();

        commandRouter.AddIdentifier(settingsProvider.Target);
        commandRouter.AddIdentifier(settingsProvider.Group);
        commandRouter.AddIdentifier("*");

        AddRoutes<OnCommand>("on");
        AddRoutes<OffCommand>("off");
        AddRoutes<StrobeCommand>("strobe");
        AddRoutes<FadeCommand>("fade");

        AddRoute<PatternCommand>("/led/pattern");
        AddRoute<WaveCommand>("/led/wave");

        commandRouter.AddRoute("/led/reload", osc =>
        {
            luxaforDeviceManager.Initialise();
        });

        commandRouter.AddRoute("/config/target", osc =>
        {
            var newTarget = osc.ReadFirstArgAsString();

            if (newTarget == null)
                return;

            commandRouter.ReplaceIdentifier(settingsProvider.Target, newTarget);
            settingsProvider.SetTarget(newTarget);
        });

        commandRouter.AddRoute("/config/group", osc =>
        {
            var newGroup = osc.ReadFirstArgAsString();

            if (newGroup == null)
                return;

            commandRouter.ReplaceIdentifier(settingsProvider.Group, newGroup);
            settingsProvider.SetTarget(newGroup);
        });

        commandListener.Subscribe(commandRouter);

        var task = commandListener.StartListening(settingsProvider.Port, stoppingToken);
        return task;
    }

    private void AddRoute<T>(string route) where T : ICommand
    {
        commandRouter.AddRoute(route, osc =>
        {
            var cmd = _serializer.Convert<T>(osc);
            luxaforDeviceManager.Run(cmd);
        });
    }

    private void AddRoutes<T>(string suffix) where T : ICommand
    {
        foreach (var led in Enum.GetValues<Led>())
        {
            var ledName = led.ToString().ToLowerInvariant();
            var path = $"/led/{ledName}/{suffix}";

            commandRouter.AddRoute(path, osc =>
            {
                var t = _serializer.Convert<T>(osc, led);
                luxaforDeviceManager.Run(t);
            });
        }
    }
}