using ApplianceWarehouse.Services.Interfaces;
using System.Globalization;

namespace ApplianceWarehouse.Controller.Commands;

public class DeleteCommand : ICommand
{
    private readonly IApplianceService _service;

    public DeleteCommand(IApplianceService service)
    {
        _service = service;
    }

    // Parameters: <Id>
    public string Execute(Request request)
    {
        if (request.Parameters.Length == 0)
            return "ERROR: Missing 'Id' parameter.";

        if (!int.TryParse(request.Parameters[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
            return "ERROR: Invalid 'Id' parameter.";
        if (id <= 0)
            return "ERROR: Id must be positive.";

        var existing = _service.GetById(id);
        if (existing == null)
            return $"ERROR: Appliance with ID {id} not found.";

        _service.Delete(id);
        return $"OK: Deleted appliance with ID {id}.";
    }
}
