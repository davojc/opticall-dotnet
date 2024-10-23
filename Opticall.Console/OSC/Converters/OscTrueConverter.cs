using Opticall.Console.OSC.Types;

namespace Opticall.Console.OSC.Converters;

public class OscTrueConverter : IConverter<OscTrue>
{
    public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out OscTrue value)
    {
        value = OscTrue.True;
        return dWords;
    }

    public IEnumerable<DWord> Serialize(OscTrue value)
    {
        return new DWord[0];
    }
}