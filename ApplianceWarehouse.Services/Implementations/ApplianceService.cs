using ApplianceWarehouse.Domain.Entities;
using ApplianceWarehouse.Domain.Repositories;
using ApplianceWarehouse.Services.Exceptions;
using ApplianceWarehouse.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace ApplianceWarehouse.Services.Implementations;

public class ApplianceService : IApplianceService
{

    public IEnumerable<Appliance> SearchByCategory(string categoryName)
{
    if (!Enum.TryParse<ApplianceCategory>(categoryName, true, out var category))
        throw new ServiceException($"Invalid category: {categoryName}");

    try
    {
        return _repository.GetAll()
                          .Where(a => a.Category == category);
    }
    catch (Exception ex)
    {
        throw new ServiceException($"Error searching appliances by category '{categoryName}'", ex);
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

    private readonly IApplianceRepository _repository;
    public ApplianceService(IApplianceRepository repository)
    {
        _repository = repository;
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