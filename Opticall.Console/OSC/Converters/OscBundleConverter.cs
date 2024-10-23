namespace Opticall.Console.OSC.Converters;

public class OscBundleConverter : IConverter<OscBundle>
{
    private readonly StringConverter stringConverter = new StringConverter();
    private readonly TimetagConverter timetagConverter = new TimetagConverter();
    private readonly IntConverter intConverter = new IntConverter();
    private readonly OscMessageConverter messageConverter = new OscMessageConverter();

    public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out OscBundle value)
    {
        var afterBundleHeader = DeserializeBundleHeader(dWords);
        var afterTimetag = DeserializeTimetag(afterBundleHeader, out var timetag);
        var afterLength = DeserializeLength(afterTimetag, out var length);
        _ = DeserializeMessages(afterLength.Take(length / 4), out var messages);
        value = new OscBundle(timetag, messages);
        return afterLength.Skip(length / 4);
    }

    public IEnumerable<DWord> Serialize(OscBundle value)
    {
        var messages = SerializeMessages(value.Messages);
        var length = messages.Count() * 4;
        return
            SerializeBundleHeader()
                .Concat(SerializeTimetag(value.Timetag))
                .Concat(SerializeLength(length))
                .Concat(messages);
    }

    private IEnumerable<DWord> DeserializeBundleHeader(IEnumerable<DWord> dWords)
    {
        return stringConverter.Deserialize(dWords, out string _);
    }

    private IEnumerable<DWord> SerializeBundleHeader()
    {
        return stringConverter.Serialize("#bundle");
    }

    private IEnumerable<DWord> DeserializeTimetag(IEnumerable<DWord> dWords, out Timetag timetag)
    {
        return timetagConverter.Deserialize(dWords, out timetag);
    }

    private IEnumerable<DWord> SerializeTimetag(Timetag timetag)
    {
        return timetagConverter.Serialize(timetag);
    }

    private IEnumerable<DWord> DeserializeLength(IEnumerable<DWord> dWords, out int length)
    {
        return intConverter.Deserialize(dWords, out length);
    }

    private IEnumerable<DWord> SerializeLength(int length)
    {
        return intConverter.Serialize(length);
    }

    private IEnumerable<DWord> DeserializeMessages(IEnumerable<DWord> dWords, out IEnumerable<OscMessage> messages)
    {
        var result = new List<OscMessage>();
        while (dWords.Any())
        {
            dWords = messageConverter.Deserialize(dWords, out OscMessage message);
            result.Add(message);
        }

        messages = result;
        return dWords;
    }

    private IEnumerable<DWord> SerializeMessages(IEnumerable<OscMessage> messages)
    {
        return messages.Select(message => messageConverter.Serialize(message)).SelectMany(dWord => dWord);
    }
}

public class IntConverter : IConverter<int>
{
    public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out int value)
    {
        value = BitConverter.ToInt32(dWords.First().Reverse().Bytes, 0);
        return dWords.Skip(1);
    }

    public IEnumerable<DWord> Serialize(int value)
    {
        yield return new DWord(BitConverter.GetBytes(value))
            .Reverse();
    }
}

public class LongConverter : IConverter<long>
{
    public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out long value)
    {
        value = BitConverter.ToInt64(
            dWords.Skip(1).First().Reverse().Bytes
                .Concat(dWords.First().Reverse().Bytes).ToArray(), 0);
        return dWords.Skip(2);
    }

    public IEnumerable<DWord> Serialize(long value)
    {
        var bytes = BitConverter.GetBytes(value);
        yield return new DWord(bytes.Skip(4).Take(4))
            .Reverse();
        yield return new DWord(bytes.Take(4))
            .Reverse();
    }
}