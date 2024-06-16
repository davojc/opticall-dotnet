using CommandLine;
using Opticall;
using Opticall.Config;
using Opticall.Luxafor;
using Opticall.IO;
using Opticall.Messaging.Signals;
using Opticall.Processors;
using Microsoft.Extensions.Configuration;

var signalSerialiser = new JsonSignalSerialiser();
var commandBuilder = new CommandBuilder();

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
        .AddJsonFile(rootArgs.ConfigFile)
        //.AddCommandLine(args)
        .Build();

Settings? settings = config.GetRequiredSection("Settings").Get<Settings>();

var serverMode = rootArgs.ServerMode ?? settings.Mode;

switch(serverMode)
{
    case Mode.Server:

        var luxafor = LuxaforDevice.Find();

        luxafor.RunDirect(new byte[]{1,255,255,0,0,0,0,0});

        var controller = new SignalProcessor(luxafor, commandBuilder, settings.Signal.Name, settings.Signal.Group);

        using(var receiver = new UdpListener<ISignalTopic, SignalType>(settings.Network, signalSerialiser))
        {
            receiver.Subscribe(controller);

            var task = receiver.StartListening();
            Console.WriteLine("Listening for incoming commands");
            Console.ReadLine();

            // Stop the server
            receiver.Stop();

            // Wait for the listening task to complete
            await task;
        }

        luxafor.Run(commandBuilder.Build(new OffSignal()));

        break;
    case Mode.Monitor:
        Console.WriteLine("Monitor");
        break;

    default:
        
        var sendArgs = argsParser.ParseArguments<SendArgs>(args).Value;

        if(sendArgs == null || sendArgs.Signal == null)
        {
            Console.WriteLine("Couldn't parse arguments.");
            break;
        }

        using(var sender = new UdpSender<ISignalTopic, SignalType>(settings.Network, signalSerialiser))
        {
            var signalToSend = signalSerialiser.Deserialise(sendArgs.Type, sendArgs.Signal);

            if(signalToSend != null)
            {
                await sender.Send(sendArgs.Type, signalToSend);
            }
        }

        break;
}
