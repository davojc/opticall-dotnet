using System.Reflection;
using System.Text.Json;

namespace Opticall.Messaging.Signals;

public class JsonSignalSerialiser : IContentSerialiser<ISignalTopic, SignalType>
{
    private readonly JsonSerializerOptions _options;
    private readonly Dictionary<SignalType, Type> _signalTypes;

    public JsonSignalSerialiser()
    {
        _options = new JsonSerializerOptions 
                        {   
                            WriteIndented = false, 
                            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.Strict,
                            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never
                        };

        _signalTypes = [];

        foreach(var type in typeof(JsonSignalSerialiser).Assembly.GetTypes())
        {
             var attribute = type.GetCustomAttribute<SignalAttribute>();

            if(attribute == null)
                continue;

            _signalTypes[attribute.Signal] = type;
        }
    }

    public ISignalTopic? Deserialise(SignalType contentType, string signal)
    {
        if(_signalTypes.TryGetValue(contentType, out Type? type) && type != null)
            return JsonSerializer.Deserialize(signal, type) as ISignalTopic;

        return null;
    }

    public string Serialise(ISignalTopic? signal)
    {
        if(signal == null)
        {
            return string.Empty;
        }

        return JsonSerializer.Serialize(signal, signal.GetType(), _options);
    }
}
