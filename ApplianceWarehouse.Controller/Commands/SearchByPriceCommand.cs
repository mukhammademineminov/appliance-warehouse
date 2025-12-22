using ApplianceWarehouse.Services.Interfaces;
using System.Globalization;

namespace ApplianceWarehouse.Controller.Commands;

public class SearchByPriceCommand : ICommand
{
    private readonly IApplianceService _service;

    public SearchByPriceCommand(IApplianceService service)
    {
        _service = service;
    }

    // Parameters: <minPrice>;<maxPrice>
    public string Execute(Request request)
    {
        if (request.Parameters.Length < 2)
            return "ERROR: Missing 'minPrice' or 'maxPrice' parameter.";

        if (!decimal.TryParse(request.Parameters[0], NumberStyles.Number, CultureInfo.InvariantCulture, out var min))
            return "ERROR: Invalid 'minPrice' parameter.";

        if (!decimal.TryParse(request.Parameters[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var max))
            return "ERROR: Invalid 'maxPrice' parameter.";

        var items = _service.SearchByPriceRange(min, max)?.ToList() ?? new List<Domain.Entities.Appliance>();
        if (!items.Any())
            return $"OK: No appliances found in price range {min} - {max}.";

        return string.Join("\n", items.Select(a => $"OK: {a.Id} | {a.Name} | {a.Price}"));
    }
}
