using ApplianceWarehouse.Controller;
using ApplianceWarehouse.Controller.Commands;
using ApplianceWarehouse.Controller.Factory;
using ApplianceWarehouse.Domain.Entities;
using ApplianceWarehouse.Services.Interfaces;

namespace ApplianceWarehouse.Tests;

public class CommandTests
{
    private class FakeService : IApplianceService
    {
        private readonly List<Appliance> _items;

        public FakeService(IEnumerable<Appliance> seed)
        {
            _items = seed.ToList();
        }

        public IEnumerable<Appliance> GetAll() => _items;
        public IEnumerable<Appliance> SearchByCategory(string category) => _items.Where(a => a.Category.ToString().Equals(category, StringComparison.OrdinalIgnoreCase));
        public IEnumerable<Appliance> SearchByBrand(string brand) => _items.Where(a => a.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase));
        public IEnumerable<Appliance> SearchByPriceRange(decimal minPrice, decimal maxPrice) => _items.Where(a => a.Price >= minPrice && a.Price <= maxPrice);
        public Appliance? GetById(int id) => _items.FirstOrDefault(a => a.Id == id);
        public void Add(Appliance appliance) => _items.Add(appliance);
        public void Update(Appliance appliance)
        {
            var idx = _items.FindIndex(a => a.Id == appliance.Id);
            if (idx >= 0) _items[idx] = appliance;
        }
        public void Delete(int id)
        {
            var item = _items.FirstOrDefault(a => a.Id == id);
            if (item != null) _items.Remove(item);
        }
    }

    private static List<Appliance> Seed() => new()
    {
        new Appliance(1, "LG Fridge", "LG", 450m, ApplianceCategory.Refrigerator, 5),
        new Appliance(2, "Samsung Washer", "Samsung", 320m, ApplianceCategory.WashingMachine, 3),
        new Appliance(3, "Bosch Dishwasher", "Bosch", 280m, ApplianceCategory.Dishwasher, 2)
    };

    [Fact]
    public void GetAll_Sorts_ByPriceDesc_WhenRequested()
    {
        var service = new FakeService(Seed());
        var cmd = new GetAllCommand(service);

        var result = cmd.Execute(new Request("GETALL;price;desc"));

        Assert.Contains("OK: 1 | LG Fridge | 450", result);
        var lines = result.Split('\n');
        Assert.Equal("OK: 1 | LG Fridge | 450", lines[0]);
        Assert.Equal("OK: 2 | Samsung Washer | 320", lines[1]);
        Assert.Equal("OK: 3 | Bosch Dishwasher | 280", lines[2]);
    }

    [Fact]
    public void SearchByCategory_AcceptsAlias_Washer()
    {
        var service = new FakeService(Seed());
        var cmd = new SearchByCategoryCommand(service);

        var result = cmd.Execute(new Request("SEARCHBYCATEGORY;Washer"));

        Assert.Contains("Samsung Washer", result);
    }

    [Fact]
    public void SearchByBrand_FiltersByBrand()
    {
        var service = new FakeService(Seed());
        var cmd = new SearchByBrandCommand(service);

        var result = cmd.Execute(new Request("SEARCHBYBRAND;Samsung"));

        Assert.DoesNotContain("LG Fridge", result);
        Assert.Contains("Samsung Washer", result);
    }

    [Fact]
    public void AddCommand_AssignsNextId_AndAdds()
    {
        var service = new FakeService(Seed());
        var cmd = new AddCommand(service);

        var result = cmd.Execute(new Request("ADD;New Item;BrandX;Refrigerator;100;1"));

        Assert.Contains("OK: Added appliance", result);
        Assert.NotNull(service.GetById(4));
    }

    [Fact]
    public void AddCommand_Blocks_Duplicate_NameBrandCategory()
    {
        var service = new FakeService(Seed());
        var cmd = new AddCommand(service);

        var result = cmd.Execute(new Request("ADD;LG Fridge;LG;Refrigerator;500;2"));

        Assert.Contains("ERROR: Duplicate appliance", result);
    }

    [Fact]
    public void UpdateCommand_Blocks_Duplicate_NameBrandCategory()
    {
        var service = new FakeService(Seed());
        var cmd = new UpdateCommand(service);

        // Try to make item 3 identical to item 1 (Name+Brand+Category)
        var result = cmd.Execute(new Request("UPDATE;3;LG Fridge;LG;Refrigerator;300;1"));

        Assert.Contains("ERROR: Duplicate appliance", result);
    }
}