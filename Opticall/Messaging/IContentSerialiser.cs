namespace Opticall.Messaging;

public interface IContentSerialiser<T, M> where M : struct, Enum
{
    T? Deserialise(M contentType, string content);

    string Serialise(T? content);
}
