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
            { CommandName.SearchByCategory, new SearchByCategoryCommand(service) },
            { CommandName.SearchByBrand, new SearchByBrandCommand(service) },
            { CommandName.SearchByPrice, new SearchByPriceCommand(service) },
            { CommandName.Add, new AddCommand(service) },
            { CommandName.Update, new UpdateCommand(service) },
            { CommandName.Delete, new DeleteCommand(service) }
        };
    }

    public ICommand GetCommand(string name)
    {
        if (Enum.TryParse<CommandName>(name, true, out var commandName))
        {
            if (commands.TryGetValue(commandName, out var command))
                return command;

            throw new Exceptions.UnknownCommandException($"Command '{name}' is recognized but not implemented.");
        }

        throw new Exceptions.UnknownCommandException($"Unknown command: {name}");
    }
}