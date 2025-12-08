using ApplianceWarehouse.Controller.Commands;
using ApplianceWarehouse.Services.Interfaces;

namespace ApplianceWarehouse.Controller.Factory;

public class CommandProvider
{
    private readonly Dictionary<CommandName, ICommand> commands;

    public CommandProvider(IApplianceService service)
    {
        commands = new Dictionary<CommandName, ICommand>()
        {
            { CommandName.GetAll, new GetAllCommand(service) },
            { CommandName.GetById, new GetByIdCommand(service) },
            { CommandName.WrongRequest, new WrongCommand() }
        };
    }

    public ICommand GetCommand(string name)
    {
        if (Enum.TryParse<CommandName>(name, true, out var commandName))
        {
            return commands[commandName];
        }

        return commands[CommandName.WrongRequest];
    }
}