using ApplianceWarehouse.Services.Interfaces;

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
        var items = _service.GetAll()?.ToList() ?? new List<Domain.Entities.Appliance>();
        if (!items.Any())
            return "OK: No appliances found.";

        // If no sort params provided, return default listing
        if (request.Parameters.Length == 0 || string.IsNullOrWhiteSpace(request.Parameters[0]))
        {
            return string.Join("\n", items.Select(a => $"OK: {a.Id} | {a.Name} | {a.Price}"));
        }

        var field = request.Parameters[0].Trim().ToLowerInvariant();
        var order = request.Parameters.Length > 1 ? request.Parameters[1].Trim().ToLowerInvariant() : "asc";

        // Accept multiple descending synonyms (dsc, desc, descending, d)
        var isDesc = order.StartsWith('d');

        IOrderedEnumerable<Domain.Entities.Appliance> ordered;
        switch (field)
        {
            case "id":
                ordered = isDesc ? items.OrderByDescending(a => a.Id) : items.OrderBy(a => a.Id);
                break;
            case "name":
                ordered = isDesc ? items.OrderByDescending(a => a.Name) : items.OrderBy(a => a.Name);
                break;
            case "brand":
                ordered = isDesc ? items.OrderByDescending(a => a.Brand) : items.OrderBy(a => a.Brand);
                break;
            case "price":
                ordered = isDesc ? items.OrderByDescending(a => a.Price) : items.OrderBy(a => a.Price);
                break;
            case "category":
                ordered = isDesc ? items.OrderByDescending(a => a.Category) : items.OrderBy(a => a.Category);
                break;
            case "stock":
            case "stockquantity":
                ordered = isDesc ? items.OrderByDescending(a => a.StockQuantity) : items.OrderBy(a => a.StockQuantity);
                break;
            default:
                return $"ERROR: Unknown sort field: {request.Parameters[0]}";
        }

        return string.Join("\n", ordered.Select(a => $"OK: {a.Id} | {a.Name} | {a.Price}"));
    }
}