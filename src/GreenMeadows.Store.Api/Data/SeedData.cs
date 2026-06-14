using GreenMeadows.Store.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace GreenMeadows.Store.Api.Data;

/// <summary>
/// Static catalogue seeded via migrations so a fresh database is usable immediately
/// </summary>
public static class SeedData
{
    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Name = "Aurora X1",
                Description = "6.7\" flagship smartphone, 256GB, triple camera.",
                Price = 899.00m,
                ImageUrl = "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=600&q=80&auto=format",
                StockQuantity = 25,
            },
            new Product
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Name = "Nimbus 5",
                Description = "6.1\" mid-range smartphone, 128GB, dual camera.",
                Price = 499.00m,
                ImageUrl = "https://images.unsplash.com/photo-1592750475338-74b7b21085ab?w=600&q=80&auto=format",
                StockQuantity = 60,
            },
            new Product
            {
                Id = new Guid("33333333-3333-3333-3333-333333333333"),
                Name = "Pixela Lite",
                Description = "6.0\" budget smartphone, 64GB, long battery life.",
                Price = 299.00m,
                ImageUrl = "https://images.unsplash.com/photo-1598327105666-5b89351aff97?w=600&q=80&auto=format",
                StockQuantity = 100,
            },
            new Product
            {
                Id = new Guid("44444444-4444-4444-4444-444444444444"),
                Name = "Quantum Pro",
                Description = "6.8\" pro smartphone, 512GB, 120Hz display.",
                Price = 1099.00m,
                ImageUrl = "https://images.unsplash.com/photo-1605236453806-6ff36851218e?w=600&q=80&auto=format",
                StockQuantity = 40,
            },
            new Product
            {
                Id = new Guid("55555555-5555-5555-5555-555555555555"),
                Name = "Eon Mini",
                Description = "5.4\" compact smartphone, 128GB, lightweight.",
                Price = 349.00m,
                ImageUrl = "https://images.unsplash.com/photo-1567581935884-3349723552ca?w=600&q=80&auto=format",
                StockQuantity = 3,
            },
            new Product
            {
                Id = new Guid("66666666-6666-6666-6666-666666666666"),
                Name = "Solis Z",
                Description = "6.5\" smartphone, 256GB, fast wireless charging.",
                Price = 749.00m,
                ImageUrl = "https://images.unsplash.com/photo-1580910051074-3eb694886505?w=600&q=80&auto=format",
                StockQuantity = 12,
            });
    }
}
