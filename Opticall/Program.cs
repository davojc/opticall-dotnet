using CommandLine;
using Opticall;
using Opticall.Config;
using Opticall.Luxafor;
using Opticall.Messaging;
using Opticall.IO;
using HidSharp.Experimental;


var networkSettings = new NetworkSettings() { BindingAddress = "192.168.3.255", Port = 7654 };
var serialiser = new MessageSerialiser();

var result = Parser.Default.ParseArguments<Options>(args);

switch(result.Value.ServerMode)
{
    case Mode.Server:

        var luxafor = LuxaforDevice.Find();
        var controller = new LuxaforController(luxafor);

        using(var receiver = new UdpListener(networkSettings, serialiser))
        {
            receiver.Subscribe(controller);

            var task = receiver.StartListening();
            Console.WriteLine("Listening for incoming commands");
            Console.ReadLine();

            // Stop the server
            receiver.Stop();

            // Wait for the listening task to complete
            await task;
        }

        luxafor.Off();


        break;
    case Mode.Monitor:
        Console.WriteLine("Monitor");
        break;

    default:
        
        using(var sender = new UdpSender<Message>(networkSettings, serialiser))
        {
            Console.WriteLine("sender");
        }

        break;
}
