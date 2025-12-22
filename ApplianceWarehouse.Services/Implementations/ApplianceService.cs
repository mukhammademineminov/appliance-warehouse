using ApplianceWarehouse.Domain.Entities;
using ApplianceWarehouse.Domain.Repositories;
using ApplianceWarehouse.Services.Exceptions;
using ApplianceWarehouse.Services.Interfaces;

namespace ApplianceWarehouse.Services.Implementations;

public class ApplianceService : IApplianceService
{

    private readonly IApplianceRepository _repository;

    public ApplianceService(IApplianceRepository repository)
    {
        _repository = repository;
    }
    
    public IEnumerable<Appliance> SearchByCategory(string category)
{
    if (!Enum.TryParse<ApplianceCategory>(category, true, out var categoryEnum))
        throw new ServiceException($"Invalid category: {category}");

    try
    {
        return _repository.GetAll()
                          .Where(a => a.Category == categoryEnum);
    }
    catch (Exception ex)
    {
        throw new ServiceException($"Error searching appliances by category '{category}'", ex);
    }
}

    public IEnumerable<Appliance> SearchByBrand(string brand)
    {
        if (string.IsNullOrWhiteSpace(brand))
            throw new ServiceException("Brand cannot be empty");

        try
        {
            return _repository.GetAll()
                              .Where(a => a.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            throw new ServiceException($"Error searching appliances by brand '{brand}'", ex);
        }
    }

    public IEnumerable<Appliance> SearchByPriceRange(decimal minPrice, decimal maxPrice)
{
    if (minPrice < 0 || maxPrice < 0)
        throw new ServiceException("Price cannot be negative");
    if (minPrice > maxPrice)
        throw new ServiceException("MinPrice cannot be greater than MaxPrice");

    try
    {
        return _repository.GetAll()
                          .Where(a => a.Price >= minPrice && a.Price <= maxPrice);
    }
    catch (Exception ex)
    {
        throw new ServiceException($"Error searching appliances by price range {minPrice}-{maxPrice}", ex);
    }
}


    public IEnumerable<Appliance> GetAll()
    {
        try
        {
            return _repository.GetAll();
        }
        catch (Exception ex)
        {
            throw new ServiceException("Error fetching all appliances", ex);
        }
    }

    public Appliance? GetById(int id)
    {
        try
        {
            return _repository.GetById(id);
        }
        catch (Exception ex)
        {
            throw new ServiceException($"Error fetching appliance with ID {id}", ex);
        }
    }

    public void Add(Appliance appliance)
    {
        if (appliance == null)
            throw new ServiceException("Appliance can not be null");
            
        if (appliance.Price < 0)
            throw new ServiceException("Invalid price value.");

        try
        {
            _repository.Add(appliance);
        }
        catch (Exception ex)
        {
            throw new ServiceException("Error adding appliance", ex);
        }
    }
    public void Update(Appliance appliance)
    {
        if (appliance == null)
            throw new ServiceException("Appliance can not be null");
        
        try
        {
            _repository.Update(appliance);
        }
        catch (Exception ex)
        {
            throw new ServiceException("Error updating appliance", ex);
        }
    }
    public void Delete(int id)
    {
        try
        {
            _repository.Delete(id);
        }
        catch (Exception ex)
        {
            throw new ServiceException("Error deleting appliance", ex);
        }
    }
}