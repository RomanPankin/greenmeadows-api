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
                Name = "Garden Spade",
                Description = "Stainless steel spade with an ash handle.",
                Price = 24.99m,
                ImageUrl = "https://placehold.co/300x300?text=Spade",
                StockQuantity = 25,
            },
            new Product
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Name = "Terracotta Pot (Large)",
                Description = "Frost-resistant 30cm terracotta planter.",
                Price = 12.50m,
                ImageUrl = "https://placehold.co/300x300?text=Pot",
                StockQuantity = 60,
            },
            new Product
            {
                Id = new Guid("33333333-3333-3333-3333-333333333333"),
                Name = "Wildflower Seed Mix",
                Description = "1kg pollinator-friendly native wildflower blend.",
                Price = 8.75m,
                ImageUrl = "https://placehold.co/300x300?text=Seeds",
                StockQuantity = 100,
            },
            new Product
            {
                Id = new Guid("44444444-4444-4444-4444-444444444444"),
                Name = "Watering Can (5L)",
                Description = "Galvanised steel watering can with a rose head.",
                Price = 18.00m,
                ImageUrl = "https://placehold.co/300x300?text=Can",
                StockQuantity = 40,
            },
            new Product
            {
                Id = new Guid("55555555-5555-5555-5555-555555555555"),
                Name = "Pruning Shears",
                Description = "Bypass secateurs with a non-slip grip.",
                Price = 15.25m,
                ImageUrl = "https://placehold.co/300x300?text=Shears",
                StockQuantity = 3,
            },
            new Product
            {
                Id = new Guid("66666666-6666-6666-6666-666666666666"),
                Name = "Compost Bin (220L)",
                Description = "Recycled-plastic compost converter.",
                Price = 39.99m,
                ImageUrl = "https://placehold.co/300x300?text=Compost",
                StockQuantity = 12,
            });
    }
}
