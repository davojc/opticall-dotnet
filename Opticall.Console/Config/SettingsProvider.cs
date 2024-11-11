using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Opticall.Console.Config;

public class SettingsProvider : ISettingsProvider
{
    private readonly ILogger<SettingsProvider> _logger;
    private IConfigurationRoot _config;
    private Settings _settings;
    private readonly IDeserializer _deserializer;
    private readonly ISerializer _serializer;
    private readonly string _yamlSettingsFile = "settings.yml";

    public string Target => _settings.Target;

    public string Group => _settings.Group;

    public int Port => _settings.Port;

    public SettingsProvider(ILogger<SettingsProvider> logger)
    {
        _logger = logger;
        _settings = new Settings();
        
        _deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        _serializer = new SerializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        using (var reader = new StreamReader(_yamlSettingsFile))
            _settings = _deserializer.Deserialize<Settings>(reader);

        _logger.LogInformation($"Target: {_settings.Target}");
        _logger.LogInformation($"Group: {_settings.Group}");
        _logger.LogInformation($"Port: {_settings.Port}");
    }

    public void SetTarget(string target)
    {
        _logger.LogInformation($"Changed target from '{_settings.Target}' to {target}");

        _settings.Target = target;

        WriteOutConfig();
    }

    public void SetGroup(string group)
    {
        _logger.LogInformation($"Changed group from '{_settings.Group}' to {group}");

        _settings.Group = group;

        WriteOutConfig();
    }

    public void SetPort(int port)
    {
        _logger.LogInformation($"Changed port from '{_settings.Port}' to {port}");

        _settings.Port = port;

        WriteOutConfig();
    }

    private void WriteOutConfig()
    {
        using (var writer = new StreamWriter(_yamlSettingsFile))
        {
            _serializer.Serialize(writer, _settings);
        }
    }
}