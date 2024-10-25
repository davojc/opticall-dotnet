using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Opticall.Console.Commands;
using Opticall.Console.Luxafor;

namespace Opticall.Console;

public class OpticallService : BackgroundService
{
    private readonly ILogger<OpticallService> _logger;
    private readonly ILuxaforDeviceManager _luxaforDeviceManager;
    private readonly ICommandRouter _commandRouter;
    private readonly ICommandListener _commandListener;

    public OpticallService(ILogger<OpticallService> logger, 
        ILuxaforDeviceManager luxaforDeviceManager, 
        ICommandRouter commandRouter, 
        ICommandListener commandListener)
    {
        _logger = logger;
        _luxaforDeviceManager = luxaforDeviceManager;
        _commandRouter = commandRouter;
        _commandListener = commandListener;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json" ?? "", optional: true)
            .AddEnvironmentVariables("Opticall_")
            .Build();

        _luxaforDeviceManager.Start();

        var target = config["Target"];
        var group = config["Group"];
        var port = config.GetValue<int>("Port");

        _logger.LogInformation($"Target: {target}");
        _logger.LogInformation($"Group: {group}");
        _logger.LogInformation($"Port: {port}");

        _commandRouter.AddIdentifier(target);
        _commandRouter.AddIdentifier(group);
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

            _commandRouter.ReplaceIdentifier(target, newTarget);

            Environment.SetEnvironmentVariable("Opticall_Target", newTarget, EnvironmentVariableTarget.User);
            _logger.LogInformation($"Changed target from '{target}' to {newTarget}");
        });

        _commandRouter.AddRoute("/config/group", osc =>
        {
            var newGroup = osc.ReadFirstArgAsString();

            if (newGroup == null)
                return;

            _commandRouter.ReplaceIdentifier(group, newGroup);

            Environment.SetEnvironmentVariable("Opticall_Group", newGroup, EnvironmentVariableTarget.User);
            _logger.LogInformation($"Changed group from '{group}' to {newGroup}");
        });

        _commandListener.Subscribe(_commandRouter);

        var task = _commandListener.StartListening(port, stoppingToken);
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