using System.Text;

namespace Opticall.Console.OSC.Converters;

public class StringConverter : IConverter<string>
{
    public IEnumerable<DWord> Deserialize(IEnumerable<DWord> dWords, out string value)
    {
        if (!dWords.Any())
        {
            value = "";
            return dWords;
        }

        var next = new string(Encoding.ASCII.GetChars(dWords.First().Bytes))
            .Replace("\0", string.Empty);
        if (dWords.First().Bytes.Any(b => b == 0))
        {
            // Terminator found
            value = next;
            return dWords.Skip(1);
        }
        else
        {
            var nextDWords = Deserialize(dWords.Skip(1), out string nextValue);
            value = next + nextValue;
            return nextDWords;
        }
    }

    public IEnumerable<DWord> Serialize(string value)
    {
        return Serialize(value.ToCharArray());
    }

    private IEnumerable<DWord> Serialize(IEnumerable<char> chars)
    {
        var firstChars = chars.Take(4);
        var dWord = new DWord(Encoding.ASCII.GetBytes(firstChars.ToArray()));
        if (firstChars.Count() < 4)
        {
            return new[] { dWord };
        }
        else
        {
            var nextChars = chars.Skip(4);
            return new[] { dWord }.Concat(Serialize(nextChars));
        }
    }
}