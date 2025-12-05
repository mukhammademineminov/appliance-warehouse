namespace ApplianceWarehouse.Domain.Entities
{
public class Appliance
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public decimal Price { get; set; }
    public ApplianceCategory Category { get; set; }
    public int StockQuantity { get; set; }

    public Appliance(int id, string name, string brand, decimal price, ApplianceCategory category, int stockQuantity)
    {
        Id = id;
        Name = name;
        Brand = brand;
        Price = price;
        Category = category;
        StockQuantity = stockQuantity;
    }
}
}