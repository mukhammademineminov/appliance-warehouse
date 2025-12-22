using Xunit;
using Microsoft.Extensions.DependencyInjection;

using ApplianceWarehouse.Domain.Repositories;
using ApplianceWarehouse.DAL.DAO;
using ApplianceWarehouse.Services.Interfaces;
using ApplianceWarehouse.Services.Implementations;

namespace ApplianceWarehouse.Tests;

public class IntegrationTests
{
    [Fact]
    public void Service_Should_Read_Data_From_File_Repository()
    {
        var services = new ServiceCollection();

        var testInventoryPath = Path.Combine(
            AppContext.BaseDirectory,
            "TestData",
            "inventory.test.txt"
        );

        services.AddSingleton<IApplianceRepository>(
            _ => new FileApplianceRepository(testInventoryPath)
        );

        services.AddSingleton<IApplianceService, ApplianceService>();

        var provider = services.BuildServiceProvider();
        var service = provider.GetRequiredService<IApplianceService>();

        // Act
        var items = service.GetAll();

        // Assert
        Assert.NotNull(items);
        Assert.NotEmpty(items);
    }
}