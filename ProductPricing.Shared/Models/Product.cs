namespace ProductPricing.Shared.Models;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal CurrentPrice { get; set; }
    public DateTime LastUpdated { get; set; }
    public List<PriceHistory>? PriceHistory { get; set; }
}