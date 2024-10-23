using HidSharp.Reports.Input;
using Microsoft.VisualBasic.CompilerServices;
using Opticall.Console.OSC;

namespace Opticall.Console.Commands;


internal class Routes
{
    private readonly string _identifier;
    private readonly HashSet<string> _routes = new HashSet<string>();
    private readonly HashSet<string> _paths = new HashSet<string>();

    public Routes(string identifier)
    {
        _identifier = identifier;
        ChangeIdentifier(identifier);
    }

    public void AddPath(string path)
    {
        _paths.Add(path);
    }

    public void ChangeIdentifier(string newIdentifier)
    {
        _routes.Clear();

        newIdentifier = newIdentifier.TrimStart('/');
        newIdentifier = newIdentifier.TrimEnd('/');

        foreach (var path in _paths)
        {
            _routes.Add($"{newIdentifier}{path}");
        }
    }

    public IEnumerable<string> GetRoutes()
    {
        return _routes;
    }

}

public class CommandRouter : ICommandRouter
{
    private readonly HashSet<string> _identifiers = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
    private readonly IDictionary<string, RouteSpec> _routes = new Dictionary<string, RouteSpec>(StringComparer.InvariantCultureIgnoreCase);

    private class RouteSpec(string address, Action<OscMessage> onRoute)
    {
        public string Address { get; } = address;
        public Action<OscMessage> OnRoute { get; } = onRoute;
    }

    public void ReplaceIdentifier(string identifier, string newIdentifier)
    {
        _identifiers.Remove(PrepareIdentifier(identifier));
        AddIdentifier(newIdentifier);
    }

    public void AddIdentifier(string identifier)
    {
        _identifiers.Add(PrepareIdentifier(identifier));
    }

    private string PrepareIdentifier(string identifier)
    {
        identifier = identifier.TrimStart('/');
        identifier = identifier.TrimEnd('/');

        return $"/{identifier}";
    }

    public void AddRoute(string route, Action<OscMessage> onRoute)
    {
        var routeSpec = new RouteSpec(route, onRoute);
        _routes.Add(route, routeSpec);
    }

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(OscMessage message)
    {
        var firstIndex = message.Address.Value.IndexOf('/');

        if (firstIndex == -1)
            return;

        var secondIndex = message.Address.Value.IndexOf('/', firstIndex + 1);

        if (secondIndex == -1)
            return;

        var identifier = message.Address.Value[..secondIndex];

        if (!_identifiers.Contains(identifier))
        {
            return;
        }

        var route = message.Address.Value[(secondIndex)..];

        if (_routes.TryGetValue(route, out var rs))
        {
             rs.OnRoute(message);
        }
    }
}