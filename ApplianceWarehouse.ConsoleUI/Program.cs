using ApplianceWarehouse.DAL.Factories;
using ApplianceWarehouse.Domain.Entities;
using ApplianceWarehouse.Services.Factories;

var repoFactory = new ApplianceRepositoryFactory("inventory.txt");
var repo = repoFactory.CreateRepository();

var serviceFactory = new ApplianceServiceFactory(repo);
var service = serviceFactory.CreateService();

var items = service.GetAll();

foreach (var item in items)
{
    Console.WriteLine($"{item.Id} | {item.Name} | {item.Category}");
}