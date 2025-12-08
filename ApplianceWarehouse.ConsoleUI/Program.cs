using ApplianceWarehouse.DAL.Factories;
using ApplianceWarehouse.Services.Implementations;
using ApplianceWarehouse.Services.Interfaces;
using ApplianceWarehouse.Controller;
using ApplianceWarehouse.Controller.Factory;

// Repository
using System.IO;

// inventory path relative to the ConsoleUI project folder -> ../../ApplianceWarehouse.DAL/Sources/inventory.txt
var inventoryPath = Path.Combine("..", "ApplianceWarehouse.DAL", "Sources", "inventory.txt");
var repoFactory = new ApplianceWarehouse.DAL.Factories.ApplianceRepositoryFactory(inventoryPath);
var repository = repoFactory.CreateRepository();

// Service
IApplianceService service = new ApplianceService(repository);

// Controller
var commandProvider = new CommandProvider(service);
var controller = new Controller(commandProvider);

Console.WriteLine("Appliance Warehouse");
Console.WriteLine("Enter commands (GETALL, GETBYID;1, EXIT)");

while (true)
{
    Console.Write("Enter command: ");
    var input = Console.ReadLine();

    if (input == null || input.ToUpper() == "EXIT")
        break;

    var result = controller.Execute(input);
    Console.WriteLine(result);
}