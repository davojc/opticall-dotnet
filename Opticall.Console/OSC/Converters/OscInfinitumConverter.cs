using Opticall.Console.OSC.Types;

namespace Opticall.Console.OSC.Converters;

public class OscInfinitumConverter : IConverter<OscInfinitum>
{
    public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out OscInfinitum value)
    {
        value = OscInfinitum.Infinitum;
        return dWords;
    }

    public IEnumerable<DWord> Serialize(OscInfinitum value)
    {
        return new DWord[0];
    }
}