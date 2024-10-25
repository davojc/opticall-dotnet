using Microsoft.Extensions.Logging;
using Opticall.Console.OSC;

namespace Opticall.Console.Commands;

public class CommandRouter : ICommandRouter
{
    private readonly ILogger<CommandRouter> _logger;
    private readonly HashSet<string> _identifiers = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
    private readonly IDictionary<string, RouteSpec> _routes = new Dictionary<string, RouteSpec>(StringComparer.InvariantCultureIgnoreCase);

    public CommandRouter(ILogger<CommandRouter> logger)
    {
        _logger = logger;
    }

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
        _logger.LogError(error, "CommandRouter received error.");
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
            try
            {
                rs.OnRoute(message);
            }
            catch (IndexOutOfRangeException)
            {
                _logger.LogInformation("Couldn't route message, likelihood is the parameters for the command were incorrect.");
            }
        }
    }
}