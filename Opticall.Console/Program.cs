﻿using CommandLine;
using Microsoft.Extensions.Configuration;
using Opticall.Console;
using Opticall.Console.Commands;
using Opticall.Console.Config;
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
        .AddCommandLine(args)
        .Build();

var luxafor = LuxaforDevice.Find();

luxafor.RunDirect(new byte[] { 1, 255, 255, 0, 0, 0, 0, 0 });

var router = new CommandRouter();

var target = config["Target"];
var group = config["Group"]; 
var port = config.GetValue<int>("Port");

Console.WriteLine($"Target: {target}");
Console.WriteLine($"Group: {group}");
Console.WriteLine($"Port: {port}");

router.AddRoute(new OnCommand(), command => { luxafor.Run(command); }, $"/{target}/led/on", $"/{group}/led/on", "/*/led/on");
router.AddRoute(new OffCommand(), command => { luxafor.Run(command); }, $"/{target}/led/off", $"/{group}/led/off", "/*/led/off");
router.AddRoute(new PatternCommand(), command => { luxafor.Run(command); }, $"/{target}/led/pattern", $"/{group}/led/pattern", "/*/led/pattern");
router.AddRoute(new StrobeCommand(), command => { luxafor.Run(command); }, $"/{target}/led/strobe", $"/{group}/led/strobe", "/*/led/strobe");
router.AddRoute(new WaveCommand(), command => { luxafor.Run(command); }, $"/{target}/led/wave", $"/{group}/led/wave", "/*/led/wave");
router.AddRoute(new FadeCommand(), command => { luxafor.Run(command); }, $"/{target}/led/fade", $"/{group}/led/fade", "/*/led/fade");

using (var receiver = new OscUdpListener(port))
{
    receiver.Subscribe(router);

    var task = receiver.StartListening();
    Console.WriteLine("Listening for incoming commands");
    Console.ReadLine();

    // Stop the server
    receiver.Stop();

    // Wait for the listening task to complete
    await task;
}