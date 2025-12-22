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
        try
        {
            var command = _provider.GetCommand(req.Command);
            return command.Execute(req);
        }
        catch (Exceptions.UnknownCommandException ex)
        {
            return $"ERROR: {ex.Message}";
        }
        catch (Exceptions.InvalidParameterException ex)
        {
            return $"ERROR: {ex.Message}";
        }
        catch (ArgumentException ex)
        {
            return $"ERROR: {ex.Message}";
        }
    }
}