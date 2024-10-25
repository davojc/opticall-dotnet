using YamlDotNet.Serialization;

namespace Opticall.Console.Config
{
    public sealed class Settings
    {
        [YamlMember(Alias = "target")] 
        public string Target { get; set; } = "Target";


        [YamlMember(Alias = "group")] 
        public string Group { get; set; } = "Group";


        [YamlMember(Alias = "port")] 
        public int Port { get; set; } = 8765;
    }
}