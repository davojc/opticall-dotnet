
namespace Opticall.Luxafor.Commands;

public abstract class Command : ICommand
{
    private CommandType _commandByte;
    private byte _commandType;

    public Command(CommandType commandByte, byte commandType)
    {
        _commandByte = commandByte;
        _commandType = commandType;
    }

    protected virtual byte? Position1 
    { 
        get { return 0; }
    }

    protected virtual byte? Position2
    { 
        get { return 0; }
    }

    protected virtual byte? Position3
    { 
        get { return 0; }
    }

    protected virtual byte? Position4
    { 
        get { return 0; }
    }
    protected virtual byte? Position5 
    { 
        get { return 0; }
    }

    protected virtual byte? Position6 
    { 
        get { return 0; }
    }

    public byte[] GetCommandBytes()
    {
        return
                [
                    0x00,
                    (byte)_commandByte,
                    _commandType,
                    Position1 ?? (byte) 0,
                    Position2 ?? (byte) 0,
                    Position3 ?? (byte) 0,
                    Position4 ?? (byte) 0,
                    Position5 ?? (byte) 0,
                    Position6 ?? (byte) 0,
                ];
    }

    public abstract bool IsValid();
}