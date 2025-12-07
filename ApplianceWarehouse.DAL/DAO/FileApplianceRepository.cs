using ApplianceWarehouse.Domain.Repositories;
using ApplianceWarehouse.Domain.Entities;
namespace ApplianceWarehouse.DAL.DAO;
public class FileApplianceRepository : IApplianceRepository
{
    private readonly string _filePath;
    private readonly List<Appliance> _data;

    public FileApplianceRepository(string filePath)
    {
        _filePath = filePath;
    
        if (File.Exists(_filePath))
        {
            var lines = File.ReadAllLines(_filePath);
            _data = lines.Select(ParseLine).ToList();
        }
        else
        {
            _data = new List<Appliance>();
        }
    }
    private Appliance ParseLine(string line)
    {
        var parts = line.Split(';');
        return new Appliance(
            int.Parse(parts[0]),
            parts[1],
            parts[2],
            decimal.Parse(parts[3]),
            Enum.Parse<ApplianceCategory>(parts[4]),
            int.Parse(parts[5])
        );
        
    }

    public void Add(Appliance appliance)
    {
        _data.Add(appliance);
        SaveChanges();
    }

    public Appliance? GetById(int id)
    {
        return _data.FirstOrDefault(x => x.Id == id);
    }
    public IEnumerable<Appliance> GetAll()
    {
        return _data;
    }

    public void Update(Appliance appliance)
    {
        var index = _data.FindIndex(x=> x.Id == appliance.Id);
        if(index >= 0)
        {
            _data[index] = appliance;
            SaveChanges();
        }
    }

    public void Delete(int id)
    {
        var item = _data.FirstOrDefault(x=> x.Id == id);
        if(item != null)
        {
            _data.Remove(item);
            SaveChanges();
        }
    }

    private void SaveChanges()
    {
        var lines = _data.Select(a =>
            $"{a.Id};{a.Name};{a.Brand};{a.Price};{a.Category};{a.StockQuantity}"
        );

        File.WriteAllLines(_filePath, lines);
    }
}