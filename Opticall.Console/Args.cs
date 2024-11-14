using CommandLine;

namespace Opticall.Console;

public class RootArgs
{
    [Option('c', "config", Default = "appsettings.json", Required = false, HelpText = "The settings file to use.")]
    public string? ConfigFile { get; set; }

    [Option('t', "target", Required = false, HelpText = "The name of the target.")]
    public string? Target { get; set; }

    [Option('g', "group", Required = false, HelpText = "The (optional) group that the target may be a part of.")]
    public string? Group { get; set; }

    [Option('b', "bind", Required = false, HelpText = "The broadcast IP.")]
    public string? Binding { get; set; }

    [Option('p', "port", Required = false, HelpText = "The broadcast port.")]
    public int? Port { get; set; }
}