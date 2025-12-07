using ApplianceWarehouse.Domain.Repositories;
using ApplianceWarehouse.Servises.Interfaces;
using ApplianceWarehouse.Services.Implementations;

namespace ApplianceWarehouse.Services.Factories;

public class ApplianceServiceFactory
{
    private readonly IApplianceRepository _repository;

    public ApplianceServiceFactory(IApplianceRepository repository)
    {
        _repository = repository;
    }

    public IApplianceService CreateService()
    {
        return new ApplianceService(_repository);
    }
}