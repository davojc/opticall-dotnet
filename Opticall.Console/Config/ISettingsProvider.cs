namespace Opticall.Console.Config;

public interface ISettingsProvider
{
    string Target { get; }

    string Group { get; }

    int Port { get; }

    void SetTarget(string target);

    void SetGroup(string group);

    void SetPort(int port);
}