using ApplianceWarehouse.Controller.Commands;
using ApplianceWarehouse.Services.Interfaces;

namespace ApplianceWarehouse.Controller.Commands;

public class GetByIdCommand : ICommand
{
    private readonly IApplianceService _service;

    public GetByIdCommand(IApplianceService service)
    {
        _service = service;
    }

    public string Execute(Request request)
    {
        if (request.Parameters.Length == 0)
            return "ERROR: Missing 'id' parameter.";

        if (!int.TryParse(request.Parameters[0], out var id))
            return "ERROR: Invalid 'id' parameter.";

        var appliance = _service.GetById(id);

        if (appliance == null)
            return $"ERROR: Appliance with ID {id} not found.";

        return $"OK: {appliance.Id} | {appliance.Name} | {appliance.Price}";
    }
}