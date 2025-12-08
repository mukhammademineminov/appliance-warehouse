namespace ApplianceWarehouse.Controller;

public class Request
{
    public string Command { get; }
    public string[] Parameters { get; }

    public Request(string raw)
    {
        var parts = raw.Split(';', StringSplitOptions.TrimEntries);
        Command = parts[0];
        Parameters = parts.Length > 1 ? parts[1..] : Array.Empty<string>();
    }
}