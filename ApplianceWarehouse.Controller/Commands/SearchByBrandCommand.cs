using ApplianceWarehouse.Services.Interfaces;

namespace ApplianceWarehouse.Controller.Commands;

public class SearchByBrandCommand : ICommand
{
    private readonly IApplianceService _service;

    public SearchByBrandCommand(IApplianceService service)
    {
        _service = service;
    }

    public string Execute(Request request)
    {
        if (request.Parameters.Length == 0)
            return "ERROR: Missing 'brand' parameter.";

        var brand = request.Parameters[0] ?? string.Empty;
        if (string.IsNullOrWhiteSpace(brand))
            return "ERROR: Invalid 'brand' parameter.";

        var items = _service.SearchByBrand(brand)?.ToList() ?? new List<Domain.Entities.Appliance>();
        if (!items.Any())
            return $"OK: No appliances found for brand {brand}.";

        return string.Join("\n", items.Select(a => $"OK: {a.Id} | {a.Name} | {a.Price}"));
    }
}
