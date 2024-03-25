
namespace Opticall.Luxafor;

public interface ICommand
{
    public byte[] GetCommandBytes();

    bool IsValid();
}
