using System.Linq.Expressions;
using System.Text;

namespace Opticall.Messaging;

public class MessageSerialiser : IMessageSerialiser<Message>
{
    public bool TryDeserialise(byte[] data, out Message? message)
    {
        var dataAsString = Encoding.UTF8.GetString(data);

        Console.WriteLine("Received: " + dataAsString);

        var split = dataAsString.Split("|");

        message = null;

        if (string.Equals("info", split[0], StringComparison.InvariantCultureIgnoreCase))
        {
            return false;
        }

        if (!RequestTypeExtensions.TryFromString(split[0], out RequestType requestType))
        {
            Console.WriteLine("Request Type not recognised");
            return false;
        }

        if(split.Length < 2)
        {
            Console.WriteLine("No target provided.");
            return false;
        }

        var target = split[1];

        // off|target
        if(requestType == RequestType.Off)
        {
            message = new Message { Request = requestType, Target = target };
            return true;
        }

        Func<string[], int, byte?> getByte = (data, index) => {

            if(data == null || data.Length < index + 1) 
                return null;

            return Convert.ToByte(data[index]);
        };

        // pattern|target|repeat
        if(requestType == RequestType.Pattern)
        {
            message = new Message { 
                Request = requestType, 
                Target = target,
                Pattern = split[2]};
            return true;
        }

        var index = 2;

        switch(requestType)
        {
            // on|target|red|green|blue
            case RequestType.On:
                message = new Message { 
                    Request = requestType, 
                    Target = target,
                    Red = getByte(split, index++),
                    Green = getByte(split, index++),
                    Blue = getByte(split, index++),
                };
                return true;
            // pattern|target|repeat
            case RequestType.Pattern:
                message = new Message { 
                    Request = requestType, 
                    Target = target,
                    Red = getByte(split, index++),
                    Green = getByte(split, index++),
                    Blue = getByte(split, index++),
                };
                return true;
            
            // fade|target|red|green|blue|speed
            case RequestType.Fade:
                message = new Message { 
                    Request = requestType, 
                    Target = target,
                    Red = getByte(split, index++),
                    Green = getByte(split, index++),
                    Blue = getByte(split, index++),
                    Speed = getByte(split, index++)
                };
                return true;

            // flash|target|red|green|blue|speed|repeat
            case RequestType.Flash:
                message = new Message { 
                    Request = requestType, 
                    Target = target,
                    Red = getByte(split, index++),
                    Green = getByte(split, index++),
                    Blue = getByte(split, index++),
                    Speed = getByte(split, index++),
                    Repeat = getByte(split, index++)
                };
                return true;

            // wave|target|red|green|blue|speed|repeat
            case RequestType.Wave:
                message = new Message { 
                    Request = requestType, 
                    Target = target,
                    Red = getByte(split, index++),
                    Green = getByte(split, index++),
                    Blue = getByte(split, index++),
                    Speed = getByte(split, index++),
                    Repeat = getByte(split, index++)
                };
                return true;
        }

        return false; 
    }

    public byte[] Serialise(Message message)
    {
        var sb = new StringBuilder();
        sb.AppendJoin("|", message.Request.AsString(), message.Target, message.Red, message.Green, message.Blue);
        return Encoding.UTF8.GetBytes(sb.ToString());
    }
}


