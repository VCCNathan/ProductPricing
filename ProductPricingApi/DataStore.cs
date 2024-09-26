using ProductPricing.Shared.Models;

namespace ProductPricing.Api;

public static class DataStore
{
    public static List<Product> Products { get; set; } =
    [
        new()
        {
            Id = 1,
            Name = "Toy Car",
            CurrentPrice = 100.0m,
            LastUpdated = DateTime.Now.AddDays(-1),
            PriceHistory =
            [
                new PriceHistory {Price = 120.0m, Date = DateTime.Now.AddDays(-25)},
                new PriceHistory {Price = 100.0m, Date = DateTime.Now.AddDays(-45)}
            ]
        },

        new()
        {
            Id = 2,
            Name = "Toy Doll",
            CurrentPrice = 200.0m,
            LastUpdated = DateTime.Now.AddDays(-1),
            PriceHistory =
            [
                new PriceHistory {Price = 120.0m, Date = DateTime.Now.AddDays(-25)},
                new PriceHistory {Price = 100.0m, Date = DateTime.Now.AddDays(-45)}
            ]
        }
    ];
}