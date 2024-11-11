using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Opticall.Console.Commands;
using Opticall.Console.Config;
using Opticall.Console.Luxafor;

namespace Opticall.Console.Services;

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

        _commandRouter.AddIdentifier(_settingsProvider.Target);
        _commandRouter.AddIdentifier(_settingsProvider.Group);
        _commandRouter.AddIdentifier("*");

        var onCommand = new OnCommand();
        _commandRouter.AddRoute("/led/on", osc =>
        {
            var cmd = onCommand.CreateCommand(osc);
            _luxaforDeviceManager.Run(cmd);
        });

        var offCommand = new OffCommand();
        _commandRouter.AddRoute("/led/off", osc =>
        {
            var cmd = offCommand.CreateCommand(osc);
            _luxaforDeviceManager.Run(cmd);
        });

        var patternCommand = new PatternCommand();
        _commandRouter.AddRoute("/led/pattern", osc =>
        {
            var cmd = patternCommand.CreateCommand(osc);
            _luxaforDeviceManager.RunDirect(cmd);
        });

        var strobeCommand = new StrobeCommand();
        _commandRouter.AddRoute("/led/strobe", osc =>
        {
            var cmd = strobeCommand.CreateCommand(osc);
            _luxaforDeviceManager.Run(cmd);
        });

        var waveCommand = new WaveCommand();
        _commandRouter.AddRoute("/led/wave", osc =>
        {
            var cmd = waveCommand.CreateCommand(osc);
            _luxaforDeviceManager.Run(cmd);
        });

        var fadeCommand = new FadeCommand();
        _commandRouter.AddRoute("/led/fade", osc =>
        {
            var cmd = fadeCommand.CreateCommand(osc);
            _luxaforDeviceManager.Run(cmd);
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
    }
}