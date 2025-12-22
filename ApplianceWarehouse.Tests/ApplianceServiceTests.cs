using ApplianceWarehouse.Domain.Entities;
using ApplianceWarehouse.Domain.Repositories;
using ApplianceWarehouse.Services.Implementations;
using ApplianceWarehouse.Services.Exceptions;
using ApplianceWarehouse.Services.Interfaces;
using Xunit;

namespace ApplianceWarehouse.Tests;

public class ApplianceServiceTests
{
    private class FakeRepository : IApplianceRepository
    {
        private readonly List<Appliance> _items;

        public FakeRepository(IEnumerable<Appliance> seed)
        {
            _items = seed.ToList();
        }

        public IEnumerable<Appliance> GetAll() => _items;

        public Appliance? GetById(int id) =>
            _items.FirstOrDefault(a => a.Id == id);

        public void Add(Appliance appliance) =>
            _items.Add(appliance);

        public void Update(Appliance appliance)
        {
            var index = _items.FindIndex(a => a.Id == appliance.Id);
            if (index >= 0)
                _items[index] = appliance;
        }

        public void Delete(int id)
        {
            var item = _items.FirstOrDefault(a => a.Id == id);
            if (item != null)
                _items.Remove(item);
        }
    }

    private static List<Appliance> Seed() => new()
    {
        new Appliance(1, "LG Fridge", "LG", 450m, ApplianceCategory.Refrigerator, 5),
        new Appliance(2, "Samsung Washer", "Samsung", 320m, ApplianceCategory.WashingMachine, 3)
    };

    [Fact]
    public void Add_Should_Throw_Exception_When_Price_Is_Negative()
    {
        // Arrange
        var repo = new FakeRepository(Seed());
        IApplianceService service = new ApplianceService(repo);

        var appliance = new Appliance(
            id: 0,
            name: "Broken Item",
            brand: "Test",
            price: -100m,
            category: ApplianceCategory.Refrigerator,
            stockQuantity: 1
        );

        // Act + Assert
        var ex = Assert.Throws<ServiceException>(() => service.Add(appliance));
        Assert.Contains("price", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}