using Opticall.Console.Luxafor;

namespace Opticall.Console.Command;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class CommandAttribute(CommandType commandType) : Attribute
{
    public CommandType CommandType { get; } = commandType;
}