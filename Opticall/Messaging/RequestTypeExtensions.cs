namespace Opticall.Messaging;

public static class RequestTypeExtensions 
{
    public static string AsString(this RequestType commandType)
    {
        return commandType.ToString().ToLowerInvariant();
    }

    public static bool TryFromString(string commandTypeString, out RequestType commandType)
    {
        return Enum.TryParse(commandTypeString, true, out commandType);
    }
}


