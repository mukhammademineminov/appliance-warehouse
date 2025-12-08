using ApplianceWarehouse.Controller.Commands;
using ApplianceWarehouse.Services.Interfaces;
using System.Linq;

namespace ApplianceWarehouse.Controller.Commands;

public class GetAllCommand : ICommand
{
    private readonly IApplianceService _service;

    public GetAllCommand(IApplianceService service)
    {
        _service = service;
    }

    public string Execute(Request request)
    {
        var items = _service.GetAll()?.ToList() ?? new List<ApplianceWarehouse.Domain.Entities.Appliance>();
        if (!items.Any())
            return "OK: No appliances found.";

        return string.Join("\n", items.Select(a => $"OK: {a.Id} | {a.Name} | {a.Price}"));
    }
}