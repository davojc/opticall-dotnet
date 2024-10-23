using Opticall.Console.OSC;

namespace Opticall.Console.Commands;

public class CommandRouter : ICommandRouter
{
    private readonly IDictionary<string, RouteSpec> _routes = new Dictionary<string, RouteSpec>(StringComparer.InvariantCultureIgnoreCase);

    private class RouteSpec(string address, Command command, Action<byte[]> onRoute)
    {
        public string Address { get; } = address;
        public Command Command { get; } = command;
        public Action<byte[]> OnRoute { get; } = onRoute;
    }

    public void AddRoute(Command command, Action<byte[]> onRoute, string route, params string?[] alternates)
    {
        var routeSpec = new RouteSpec(route, command, onRoute);

        _routes.Add(route, routeSpec);

        foreach (var alternate in alternates)
        {
            if (alternate == null)
            {
                continue;
            }

            var altRouteSpec = new RouteSpec(alternate, command, onRoute);
            _routes.Add(alternate, altRouteSpec);
        }
    }

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(OscMessage message)
    {
        if (_routes.TryGetValue(message.Address.Value, out var rs))
        {
            var command = rs.Command.CreateCommand(message);
            rs.OnRoute(command);
        }
    }
}