using CommandLine;
using Microsoft.Extensions.Configuration;
using Opticall.Console;
using Opticall.Console.Commands;
using Opticall.Console.IO;
using Opticall.Console.Luxafor;

var argsParser = new Parser(settings => {
    settings.HelpWriter = Console.Error;
    settings.IgnoreUnknownArguments = true;
});

var result = argsParser.ParseArguments<RootArgs>(args);

if(result.Errors.Any())
{
    foreach(var error in result.Errors)
    {
        Console.WriteLine(error);
    }

    Environment.Exit(-1);
}

var rootArgs = result.Value;

var config = new ConfigurationBuilder()
        .AddJsonFile(rootArgs.ConfigFile ?? "", optional: true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .Build();

var cancellation = new CancellationTokenSource();

var luxafor = await LuxaforDevice.FindAsync(cancellation);

luxafor.RunDirect(new byte[] { 0, (byte)CommandType.Pattern, 1, 5 });

var router = new CommandRouter();

var target = config["Target"];
var group = config["Group"]; 
var port = config.GetValue<int>("Port");

Console.WriteLine($"Target: {target}");
Console.WriteLine($"Group: {group}");
Console.WriteLine($"Port: {port}");

router.AddIdentifier(target);
router.AddIdentifier(group);

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


using (var receiver = new OscUdpListener(port, cancellation))
{
    AppDomain.CurrentDomain.ProcessExit += (sender, args) => {
        receiver.Stop();
    };


    Console.CancelKeyPress += (sender, args) => {
        receiver.Stop();
        args.Cancel = true;
    };

    receiver.Subscribe(router);

    var task = receiver.StartListening();
    Console.WriteLine("Listening for incoming commands");
    Console.ReadLine();

    // Wait for the listening task to complete
    await task;
}