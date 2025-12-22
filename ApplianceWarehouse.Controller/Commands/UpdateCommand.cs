using ApplianceWarehouse.Services.Interfaces;
using System.Globalization;

namespace ApplianceWarehouse.Controller.Commands;

public class UpdateCommand : ICommand
{
    private readonly IApplianceService _service;

    public UpdateCommand(IApplianceService service)
    {
        _service = service;
    }

    // Parameters: <Id>;<Name>;<Brand>;<Category>;<Price>;<StockQuantity>
    public string Execute(Request request)
    {
        if (request.Parameters.Length < 6)
            return "ERROR: Missing parameters. Expected: Id;Name;Brand;Category;Price;StockQuantity";

        if (!int.TryParse(request.Parameters[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
            return "ERROR: Invalid 'Id' parameter.";
        if (id <= 0)
            return "ERROR: Id must be positive.";

        var name = request.Parameters[1]?.Trim() ?? string.Empty;
        var brand = request.Parameters[2]?.Trim() ?? string.Empty;
        var categoryRaw = request.Parameters[3] ?? string.Empty;

        if (string.IsNullOrWhiteSpace(name))
            return "ERROR: Invalid 'Name' parameter.";
        if (string.IsNullOrWhiteSpace(brand))
            return "ERROR: Invalid 'Brand' parameter.";
        if (string.IsNullOrWhiteSpace(categoryRaw))
            return "ERROR: Invalid 'Category' parameter.";

        if (!Enum.TryParse<Domain.Entities.ApplianceCategory>(categoryRaw, true, out var category))
            return $"ERROR: Unknown category: {categoryRaw}";

        if (!decimal.TryParse(request.Parameters[4], NumberStyles.Number, CultureInfo.InvariantCulture, out var price))
            return "ERROR: Invalid 'Price' parameter.";
        if (price < 0)
            return "ERROR: Price cannot be negative.";

        if (!int.TryParse(request.Parameters[5], NumberStyles.Integer, CultureInfo.InvariantCulture, out var stock))
            return "ERROR: Invalid 'StockQuantity' parameter.";
        if (stock < 0)
            return "ERROR: StockQuantity cannot be negative.";

        var existing = _service.GetById(id);
        if (existing == null)
            return $"ERROR: Appliance with ID {id} not found.";

        // Duplicate check against other items
        var all = _service.GetAll()?.ToList() ?? new List<Domain.Entities.Appliance>();
        if (all.Any(a => a.Id != id
                      && a.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                      && a.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase)
                      && a.Category == category))
        {
            return "ERROR: Duplicate appliance (Name+Brand+Category already exists).";
        }

        var updated = new Domain.Entities.Appliance(id, name, brand, price, category, stock);
        _service.Update(updated);

        return $"OK: Updated appliance {updated.Id} | {updated.Name} | {updated.Price}";
    }
}
