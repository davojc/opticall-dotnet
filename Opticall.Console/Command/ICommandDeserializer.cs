using Newtonsoft.Json;
using Opticall.Console.Luxafor;
using Opticall.Console.OSC;

namespace Opticall.Console.Command
{
    public class OscCommandParser
    {
        public IEnumerable<(string key, object value)> Parse(OscMessage oscMessage)
        {
            byte r = 0;
            byte g = 0;
            byte b = 0;

            var colorReturned = false;

            foreach (var arg in oscMessage.Arguments)
            {
                if (arg is string)
                {
                    var kvp = arg.ToString()!.Split(":");

                    if (kvp.Length != 2)
                        continue;

                    var value = kvp[1];

                    switch (kvp[0])
                    {
                        case "color":
                            yield return ("color", value);
                            colorReturned = true;
                            break;

                        case "wave":
                            if (byte.TryParse(value, out var wave))
                                yield return ("wave", (WaveType) wave);
                            else if (Enum.TryParse(value, true, out WaveType waveType))
                                yield return ("wave", waveType);
                            break;

                        case "led":
                            if (byte.TryParse(value, out var led))
                                yield return ("led", (Led)led);
                            else if (Enum.TryParse(value, true, out WaveType ledTarget))
                                yield return ("led", ledTarget);
                            break;

                        case "pattern":

                            if (byte.TryParse(value, out var pattern))
                                yield return ("pattern", (PatternType) pattern);
                            else if (Enum.TryParse(value, true, out PatternType patternType))
                                yield return ("pattern", patternType);
                            break;

                        case "repeat":
                            if(byte.TryParse(value, out var repeat))
                                yield return ("repeat", repeat);
                            break;

                        case "time":
                            if (byte.TryParse(value, out var time))
                                yield return ("time", time);
                            break;

                        case "speed":
                            if (byte.TryParse(value, out var speed))
                                yield return ("speed", speed);
                            break;

                        case "r":

                            byte.TryParse(value, out r);
                            break;

                        case "g":
                            
                            byte.TryParse(value, out g);
                            break;

                        case "b":
                            byte.TryParse(value, out b);
                            break;
                    }
                }
            }

            if (!colorReturned)
            {
                var hexColor = ColorConverter.RgbToHex(r, g, b);
                yield return ("color", hexColor);
            }
        }
    }

    public interface ICommandDeserializer 
    {
        TResult Convert<TResult>(OscMessage oscMessage, Led? led = null) where TResult : ICommand;
    }

    public class CommandOscDeserializer : ICommandDeserializer
    {
        public TResult Convert<TResult>(OscMessage oscMessage, Led? led = null) where TResult : ICommand
        {
            var parser = new OscCommandParser();
            var dictionary = parser.Parse(oscMessage).ToDictionary(e => e.key, e => e.value);

            if (led.HasValue)
                dictionary.TryAdd("led", led);

            var json = JsonConvert.SerializeObject(dictionary);
            return JsonConvert.DeserializeObject<TResult>(json);
        }
    }

    public interface ICommand
    {
        IEnumerable<byte> ToBytes();
    }
}
