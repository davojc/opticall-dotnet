using System.Text.Json.Serialization;
using Opticall.Console.Luxafor;

namespace Opticall.Console.Command.Commands;

public class OffCommand : ICommand
{
    [JsonPropertyName("led")]
    public Led Led { get; set; }

    public IEnumerable<byte> ToBytes()
    {
        yield return 0;
        yield return (byte)CommandType.Color;
        yield return (byte)Led;

        yield return 0;
        yield return 0;
        yield return 0;
        yield return 0;
        yield return 0;
        yield return 0;
    }
}