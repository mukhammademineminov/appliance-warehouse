using ApplianceWarehouse.Controller.Factory;

namespace ApplianceWarehouse.Controller;

public class Controller
{
    private readonly CommandProvider _provider;
    public Controller(CommandProvider provider)
    {
        _provider = provider;

    }

    public string Execute(string rawRawquest)
    {
        var req = new Request(rawRawquest);
        var command = _provider.GetCommand(req.Command);
        return command.Execute(req);
    }
}