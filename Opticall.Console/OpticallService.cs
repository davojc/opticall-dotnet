using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Opticall.Console.Commands;
using Opticall.Console.IO;
using Opticall.Console.Luxafor;

namespace Opticall.Console;

public class OpticallService : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json" ?? "", optional: true)
        .AddEnvironmentVariables()
        .Build();

        var luxafor = new LuxaforDeviceManager();

        var router = new CommandRouter();

        var target = config["Target"];
        var group = config["Group"];
        var port = config.GetValue<int>("Port");

        System.Console.WriteLine($"Target: {target}");
        System.Console.WriteLine($"Group: {group}");
        System.Console.WriteLine($"Port: {port}");

        router.AddIdentifier(target);
        router.AddIdentifier(group);
        router.AddIdentifier("*");

        var onCommand = new OnCommand();
        router.AddRoute("/led/on", osc =>
        {
            var cmd = onCommand.CreateCommand(osc);
            luxafor.Run(cmd);
        });

        var offCommand = new OffCommand();
        router.AddRoute("/led/off", osc =>
        {
            var cmd = offCommand.CreateCommand(osc);
            luxafor.Run(cmd);
        });

        var patternCommand = new PatternCommand();
        router.AddRoute("/led/pattern", osc =>
        {
            var cmd = patternCommand.CreateCommand(osc);
            luxafor.RunDirect(cmd);
        });

        var strobeCommand = new StrobeCommand();
        router.AddRoute("/led/strobe", osc =>
        {
            var cmd = strobeCommand.CreateCommand(osc);
            luxafor.Run(cmd);
        });

        var waveCommand = new WaveCommand();
        router.AddRoute("/led/wave", osc =>
        {
            var cmd = waveCommand.CreateCommand(osc);
            luxafor.Run(cmd);
        });

        var fadeCommand = new FadeCommand();
        router.AddRoute("/led/fade", osc =>
        {
            var cmd = fadeCommand.CreateCommand(osc);
            luxafor.Run(cmd);
        });

        router.AddRoute("/config/target", osc =>
        {
            var newTarget = osc.ReadFirstArgAsString();

            if (newTarget == null)
                return;

            router.ReplaceIdentifier(target, newTarget);

            Environment.SetEnvironmentVariable("Target", newTarget);
        });

        router.AddRoute("/config/group", osc =>
        {
            var newGroup = osc.ReadFirstArgAsString();

            if (newGroup == null)
                return;

            router.ReplaceIdentifier(group, newGroup);

            Environment.SetEnvironmentVariable("Group", newGroup);
        });



        var receiver = new OscUdpListener(port, stoppingToken);
        receiver.Subscribe(router);

        var task = receiver.StartListening();
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