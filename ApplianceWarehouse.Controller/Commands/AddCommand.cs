using ApplianceWarehouse.Services.Interfaces;
using System.Globalization;

namespace ApplianceWarehouse.Controller.Commands;

public class AddCommand : ICommand
{
    private readonly IApplianceService _service;

    public AddCommand(IApplianceService service)
    {
        _service = service;
    }

    // Parameters: <Name>;<Brand>;<Category>;<Price>;<StockQuantity>
    public string Execute(Request request)
    {
        if (request.Parameters.Length < 5)
            return "ERROR: Missing parameters. Expected: Name;Brand;Category;Price;StockQuantity";

        var name = request.Parameters[0]?.Trim() ?? string.Empty;
        var brand = request.Parameters[1]?.Trim() ?? string.Empty;
        var categoryRaw = request.Parameters[2] ?? string.Empty;

        if (string.IsNullOrWhiteSpace(name))
            return "ERROR: Invalid 'Name' parameter.";
        if (string.IsNullOrWhiteSpace(brand))
            return "ERROR: Invalid 'Brand' parameter.";
        if (string.IsNullOrWhiteSpace(categoryRaw))
            return "ERROR: Invalid 'Category' parameter.";

        if (!Enum.TryParse<Domain.Entities.ApplianceCategory>(categoryRaw, true, out var category))
            return $"ERROR: Unknown category: {categoryRaw}";

        if (!decimal.TryParse(request.Parameters[3], NumberStyles.Number, CultureInfo.InvariantCulture, out var price))
            return "ERROR: Invalid 'Price' parameter.";
        if (price < 0)
            return "ERROR: Price cannot be negative.";

        if (!int.TryParse(request.Parameters[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out var stock))
            return "ERROR: Invalid 'StockQuantity' parameter.";
        if (stock < 0)
            return "ERROR: StockQuantity cannot be negative.";

        var existing = _service.GetAll()?.ToList() ?? new List<Domain.Entities.Appliance>();

        // Duplicate checks: same Name+Brand+Category considered duplicate
        if (existing.Any(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                           && a.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase)
                           && a.Category == category))
        {
            return "ERROR: Duplicate appliance (Name+Brand+Category already exists).";
        }

        var nextId = existing.Any() ? existing.Max(a => a.Id) + 1 : 1;

        var appliance = new Domain.Entities.Appliance(nextId, name, brand, price, category, stock);
        _service.Add(appliance);

        return $"OK: Added appliance {appliance.Id} | {appliance.Name} | {appliance.Price}";
    }
}
