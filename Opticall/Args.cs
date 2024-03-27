using CommandLine;
using Opticall.Messaging.Signals;

namespace Opticall
{
    public enum Mode
    {
        Send,
        Server,
        Monitor
    }

    public class RootArgs
    {
        [Option('m', "mode", Required = false, HelpText = "There are 3 options, Default - used to send commands, Server - used to listen for commands, Monitor - to keep track of what is being sent and received.")]
        public Mode? ServerMode { get; set; }

        [Option('c', "config", Default = "appsettings.json", Required = false, HelpText = "The settings file to use.")]
        public string? ConfigFile { get; set; }

        [Option('b', "bind", Required = false, HelpText = "The broadcast IP.")]
        public string? Binding { get; set; }

        [Option('p', "port", Required = false, HelpText = "The broadcast IP.")]
        public int? Port { get; set; }
    }

    public class ServerArgs
    {
        [Option('n', "name", Required = false, HelpText = "This is the name that identifies this server.")]
        public string? Target { get; set; }

        [Option('g', "group", Required = false, HelpText = "This is the group that the server is a member of.")]
        public string? Group { get; set; }
    }

    public class MonitorArgs
    {
        

    }

    public class SendArgs
    {
        [Option('s', "signal", Required = false, HelpText = "This is the signal to send ")]
        public string? Signal { get; set; }

        [Option('t', "type", Required = false, HelpText = "Required if sending message. Supported values: on, off, wave, strobe, fade, pattern")]
        public SignalType Type { get; set; }
    }
}