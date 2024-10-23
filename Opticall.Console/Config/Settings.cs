namespace Opticall.Console.Config
{
    public sealed class Settings 
    {
        public SignalSettings? Signal { get; set; }
        public NetworkSettings? Network { get; set; }
    }

    public sealed class SignalSettings
    {
        public required string? Target { get; set; }

        public required string? Group { get; set; }
    }
}