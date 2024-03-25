using CommandLine;

namespace Opticall
{
    public enum Mode
    {
        Default,
        Server,
        Monitor
    }

    public class Options
    {
        private readonly Mode _serverMode;

        public Options(Mode serverMode)
        {
            _serverMode = serverMode;
        }

        [Option('m', "mode", Required = false, HelpText = "There are 3 options, Default - used to send commands, Server - used to listen for commands, Monitor - to keep track of what is being sent and received.")]
        public Mode ServerMode
        {
            get { return _serverMode ;}
        }
    }
}