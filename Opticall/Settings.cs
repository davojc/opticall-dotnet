

public static class LEDExtensions
{
    public static byte ToByte(this LED led) 
    {
        return (byte) led;
    }
} 


public enum LED : byte
{
    All = 0xff,
    LedA = 0x41,
	LedB = 0x42,
	Led1 = 0x01,
	Led2 = 0x02,
	Led3 = 0x03,
	Led4 = 0x04,
	Led5 = 0x05,
	Led6 = 0x06
}

public enum ProdCode : byte
{
	Enable = 0x45,
	Disable = 0x44,
	Red = 0x52,
	Green = 0x47,
	Blue = 0x42,
	Cyan = 0x43,
	Magenta = 0x4d,
	Yellow = 0x59,
	White = 0x57,
	Off = 0x4f
}