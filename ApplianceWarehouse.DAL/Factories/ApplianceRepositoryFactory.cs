using ApplianceWarehouse.Domain.Repositories;
using ApplianceWarehouse.Domain.Entities;
using ApplianceWarehouse.DAL.DAO;

namespace ApplianceWarehouse.DAL.Factories
{
    public class ApplianceRepositoryFactory
    {
        private readonly string _inventoryPath;

        public ApplianceRepositoryFactory(string inventoryPath)
        {
            _inventoryPath = inventoryPath;
        }

        public IApplianceRepository CreateRepository()
        {
            return new FileApplianceRepository(_inventoryPath);
        }
    }
}
