namespace Opticall.Config
{
    public sealed class Settings 
    {
        public Mode? Mode { get; set;}
        public SignalSettings? Signal { get; set; }
        public NetworkSettings? Network { get; set; }
    }

    public sealed class SignalSettings
    {
        public required string? Name { get; set; }

        public required string? Group { get; set; }
    }
}