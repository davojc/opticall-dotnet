namespace Opticall.Config
{
    public sealed class NetworkSettings 
    { 
        public required string BindingAddress { get; set; }

        public required int Port { get; set; }
    }
}