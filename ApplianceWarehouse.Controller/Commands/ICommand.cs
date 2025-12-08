namespace ApplianceWarehouse.Controller.Commands;

public interface ICommand
{
    string Execute(Request request);
}