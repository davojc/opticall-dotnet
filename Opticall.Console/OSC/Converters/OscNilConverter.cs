using Opticall.Console.OSC.Types;

namespace Opticall.Console.OSC.Converters;

public class OscNilConverter : IConverter<OscNil>
{
    public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out OscNil value)
    {
        value = OscNil.Nil;
        return dWords;
    }

    public IEnumerable<DWord> Serialize(OscNil value)
    {
        return new DWord[0];
    }
}