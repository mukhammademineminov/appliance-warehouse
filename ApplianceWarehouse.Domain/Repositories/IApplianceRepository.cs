using ApplianceWarehouse.Domain.Entities;

namespace ApplianceWarehouse.Domain.Repositories;

    public interface IApplianceRepository
    {
        void Add(Appliance appliance);
        Appliance? GetById(int id);
        IEnumerable<Appliance> GetAll();
        void Update(Appliance appliance);
        void Delete(int id);
    }
