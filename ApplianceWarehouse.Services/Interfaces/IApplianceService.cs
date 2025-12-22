using ApplianceWarehouse.Domain.Entities;

namespace ApplianceWarehouse.Services.Interfaces;

public interface IApplianceService
{
    IEnumerable<Appliance> GetAll();
    IEnumerable<Appliance> SearchByCategory(string category);
    IEnumerable<Appliance> SearchByBrand(string brand);
    IEnumerable<Appliance> SearchByPriceRange(decimal minPrice, decimal maxPrice);
    Appliance? GetById(int id);
    void Add(Appliance appliance);
    void Update(Appliance appliance);
    void Delete(int id);
}