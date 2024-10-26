using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Opticall.Console.Commands;
using Opticall.Console.Config;
using Opticall.Console.Luxafor;

namespace Opticall.Console;

public class LedRouteBuilder(ICommandRouter commandRouter, LedCommand command, ILuxaforDeviceManager deviceManager)
{
    public LedRouteBuilder AddRoute(string path, Led led)
    {
        commandRouter.AddRoute(path, osc =>
        {
            var cmd = command.CreateCommand(led, osc);
            deviceManager.Run(cmd);
        });

        return this;
    }
}

public static class RouteExtensions
{
    public static void AddLedRoute(this ICommandRouter commandRouter, string path, Led led, LedCommand command, ILuxaforDeviceManager luxaforDeviceManager)
    {
        commandRouter.AddRoute(path, osc =>
        {
            var cmd = command.CreateCommand(led, osc);
            luxaforDeviceManager.Run(cmd);
        });
    }
}

public class OpticallService : BackgroundService
{
    private readonly ILogger<OpticallService> _logger;
    private readonly ILuxaforDeviceManager _luxaforDeviceManager;
    private readonly ICommandRouter _commandRouter;
    private readonly ICommandListener _commandListener;
    private readonly ISettingsProvider _settingsProvider;

    public OpticallService(ILogger<OpticallService> logger, 
        ILuxaforDeviceManager luxaforDeviceManager, 
        ICommandRouter commandRouter, 
        ICommandListener commandListener,
        ISettingsProvider settingsProvider)
    {
        _logger = logger;
        _luxaforDeviceManager = luxaforDeviceManager;
        _commandRouter = commandRouter;
        _commandListener = commandListener;
        _settingsProvider = settingsProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _luxaforDeviceManager.Initialise();
        _settingsProvider.Initialise();

        _commandRouter.AddIdentifier(_settingsProvider.Target);
        _commandRouter.AddIdentifier(_settingsProvider.Group);
        _commandRouter.AddIdentifier("*");

        new LedRouteBuilder(_commandRouter, new OnCommand(), _luxaforDeviceManager)
            .AddRoute("/led/all/on", Led.All)
            .AddRoute("/led/front/on", Led.Front)
            .AddRoute("/led/back/on", Led.Back)
            .AddRoute("/led/1/on", Led.One)
            .AddRoute("/led/2/on", Led.Two)
            .AddRoute("/led/3/on", Led.Three)
            .AddRoute("/led/4/on", Led.Four)
            .AddRoute("/led/5/on", Led.Five)
            .AddRoute("/led/6/on", Led.Six);

        new LedRouteBuilder(_commandRouter, new OffCommand(), _luxaforDeviceManager)
            .AddRoute("/led/all/off", Led.All)
            .AddRoute("/led/front/off", Led.Front)
            .AddRoute("/led/back/off", Led.Back)
            .AddRoute("/led/1/off", Led.One)
            .AddRoute("/led/2/off", Led.Two)
            .AddRoute("/led/3/off", Led.Three)
            .AddRoute("/led/4/off", Led.Four)
            .AddRoute("/led/5/off", Led.Five)
            .AddRoute("/led/6/off", Led.Six);

        var patternCommand = new PatternCommand();
        _commandRouter.AddRoute("/led/pattern", osc =>
        {
            var cmd = patternCommand.CreateCommand(osc);
            _luxaforDeviceManager.RunDirect(cmd);
        });

        new LedRouteBuilder(_commandRouter, new StrobeCommand(), _luxaforDeviceManager)
            .AddRoute("/led/all/strobe", Led.All)
            .AddRoute("/led/front/strobe", Led.Front)
            .AddRoute("/led/back/strobe", Led.Back)
            .AddRoute("/led/1/strobe", Led.One)
            .AddRoute("/led/2/strobe", Led.Two)
            .AddRoute("/led/3/strobe", Led.Three)
            .AddRoute("/led/4/strobe", Led.Four)
            .AddRoute("/led/5/strobe", Led.Five)
            .AddRoute("/led/6/strobe", Led.Six);

        new LedRouteBuilder(_commandRouter, new FadeCommand(), _luxaforDeviceManager)
            .AddRoute("/led/all/fade", Led.All)
            .AddRoute("/led/front/fade", Led.Front)
            .AddRoute("/led/back/fade", Led.Back)
            .AddRoute("/led/1/fade", Led.One)
            .AddRoute("/led/2/fade", Led.Two)
            .AddRoute("/led/3/fade", Led.Three)
            .AddRoute("/led/4/fade", Led.Four)
            .AddRoute("/led/5/fade", Led.Five)
            .AddRoute("/led/6/fade", Led.Six);

        var waveCommand = new WaveCommand();
        _commandRouter.AddRoute("/led/wave", osc =>
        {
            var cmd = waveCommand.CreateCommand(osc);
            _luxaforDeviceManager.Run(cmd);
        });

        _commandRouter.AddRoute("/led/reload", osc =>
        {
            _luxaforDeviceManager.Initialise();
        });

        _commandRouter.AddRoute("/config/target", osc =>
        {
            var newTarget = osc.ReadFirstArgAsString();

            if (newTarget == null)
                return;

            _commandRouter.ReplaceIdentifier(_settingsProvider.Target, newTarget);
            _settingsProvider.SetTarget(newTarget);
        });

        _commandRouter.AddRoute("/config/group", osc =>
        {
            var newGroup = osc.ReadFirstArgAsString();

            if (newGroup == null)
                return;

            _commandRouter.ReplaceIdentifier(_settingsProvider.Group, newGroup);
            _settingsProvider.SetTarget(newGroup);
        });

        _commandListener.Subscribe(_commandRouter);

        var task = _commandListener.StartListening(_settingsProvider.Port, stoppingToken);
        return task;
        /*
        while (!cancellation.IsCancellationRequested)
        {
            var command = System.Console.ReadLine();

            if (command == null)
                continue;

            var parts = command.Split(' ');

            var address = new Address(parts[0]);
            var objectArgs = new List<object>();

            foreach (var arg in parts.Skip(1))
            {
                var intArg = Convert.ToInt32(arg);
                objectArgs.Add(intArg);
            }

            var msg = new OscMessage(address, objectArgs);

            router.OnNext(msg);
        }
        */

        // Wait for the listening task to complete
    }
}