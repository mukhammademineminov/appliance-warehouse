using ApplianceWarehouse.DAL.DAO;
using ApplianceWarehouse.Services.Implementations;
using ApplianceWarehouse.Services.Interfaces;
using ApplianceWarehouse.Controller;
using ApplianceWarehouse.Controller.Factory;
using ApplianceWarehouse.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// inventory path (env or default)
var envPath = Environment.GetEnvironmentVariable("INVENTORY_PATH");
var inventoryPath = string.IsNullOrWhiteSpace(envPath)
    ? Path.Combine("..", "ApplianceWarehouse.DAL", "Sources", "inventory.txt")
    : envPath;

// Repository
services.AddSingleton<IApplianceRepository>(
    sp => new FileApplianceRepository(inventoryPath)
);

// Service
services.AddSingleton<IApplianceService, ApplianceService>();

// Controller layer
services.AddSingleton<CommandProvider>();
services.AddSingleton<Controller>();

// Build provider
var serviceProvider = services.BuildServiceProvider();

// Resolve controller
var controller = serviceProvider.GetRequiredService<Controller>();

//Console UI
Console.WriteLine(" ");
Console.WriteLine("===========================================");
Console.WriteLine("Appliance Warehouse Console Application");
Console.WriteLine("===========================================");
Console.WriteLine(" ");
Console.WriteLine("Version: 1.0");
Console.WriteLine("Created: 2025-12-06");
Console.WriteLine(" ");
Console.WriteLine("Developer: Mukhammademin Eminov");
Console.WriteLine("Email: mukhammademin_eminov@student.itpu.uz");
Console.WriteLine("-------------------------------------------");
Console.WriteLine(" ");
Console.WriteLine("Type 'HELP' to see the list of available commands.");
Console.WriteLine("Type 'EXIT' to quit the application.");
var categories = string.Join(", ", Enum.GetNames(typeof(ApplianceWarehouse.Domain.Entities.ApplianceCategory)));
var commands = new List<string>
{
    " ",
    "[VIEW & SORT]",
    "GETALL - Retrieve all appliances (no args)",
    "GETALL;<field>;<order> - Retrieve all appliances sorted by field. Fields: id,name,brand,price,category,stock. Order: asc|desc (optional, default asc)",
    "GETBYID;<id> - Retrieve an appliance by its ID with full details",
    " ",
    "[SEARCH]",
    $"SEARCHBYCATEGORY;<category> - Search appliances by category. Available categories: {categories}",
    "SEARCHBYBRAND;<brand> - Search appliances by brand (case-insensitive)",
    "SEARCHBYPRICE;<minPrice>;<maxPrice> - Search appliances within a price range",
    " ",
    "[MANAGE]",
    "ADD;<Name>;<Brand>;<Category>;<Price>;<StockQuantity> - Add a new appliance",
    "Available Categories: " + categories,
    "UPDATE;<id>;<Name>;<Brand>;<Category>;<Price>;<StockQuantity> - Update an existing appliance",
    "DELETE;<id> - Delete an appliance by its ID",
    " ",
    "HELP - Show this help message",
    "EXIT - Exit the application"
};


while (true)
{
    Console.Write("Command > ");
    var input = Console.ReadLine();

    if (input == null || input.ToUpper() == "EXIT")
        break;
    if (input.ToUpper() == "HELP")
    {
        Console.WriteLine("Available commands:");
        foreach (var cmd in commands)
        {
            Console.WriteLine(cmd);
        }
    }
    
    else {
        try
        {
            var result = controller.Execute(input);
            Console.WriteLine(result);
        }
        catch (ApplianceWarehouse.Services.Exceptions.ServiceException ex)
        {
            // Handle service-layer errors at the application boundary
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }
}