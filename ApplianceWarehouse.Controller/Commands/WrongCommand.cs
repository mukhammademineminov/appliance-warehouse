using ApplianceWarehouse.Controller.Commands;

namespace ApplianceWarehouse.Controller.Commands;

public class WrongCommand : ICommand
{
    public string Execute(Request request)
    {
        return "ERROR: Unknown command";
    }
}