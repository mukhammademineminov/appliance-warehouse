using ApplianceWarehouse.Services.Interfaces;

namespace ApplianceWarehouse.Controller.Commands;

public class SearchByCategoryCommand : ICommand
{
    private readonly IApplianceService _service;

    public SearchByCategoryCommand(IApplianceService service)
    {
        _service = service;
    }

    public string Execute(Request request)
    {
        if (request.Parameters.Length == 0)
            return "ERROR: Missing 'category' parameter.";

        var category = request.Parameters[0] ?? string.Empty;
        if (string.IsNullOrWhiteSpace(category))
            return "ERROR: Invalid 'category' parameter.";

        // Map common aliases to enum names
        var aliasMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "washer", "WashingMachine" },
            { "washingmachine", "WashingMachine" },
            { "fridge", "Refrigerator" },
            { "refrigerator", "Refrigerator" },
            { "ac", "AirConditioner" },
            { "airconditioner", "AirConditioner" }
        };

        var normalized = aliasMap.TryGetValue(category.Trim(), out var mapped) ? mapped : category;

        // Validate category
        if (!Enum.TryParse<Domain.Entities.ApplianceCategory>(normalized, true, out _))
            return $"ERROR: Unknown category: {category}";

        var items = _service.SearchByCategory(normalized)?.ToList() ?? new List<Domain.Entities.Appliance>();
        if (!items.Any())
            return $"OK: No appliances found for category {category}.";

        return string.Join("\n", items.Select(a => $"OK: {a.Id} | {a.Name} | {a.Price}"));
    }
}