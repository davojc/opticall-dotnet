namespace Opticall.Console.Requests;

public record OffRequest : IRequest
{
    public byte[] ToBytes()
    {
        var input = Enumerable.Repeat((byte)0, 0).ToArray();
        return input;
    }
}