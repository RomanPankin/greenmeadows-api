using GreenMeadows.Store.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace GreenMeadows.Store.Api.Data;

public class StoreDbContext(DbContextOptions<StoreDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(e =>
        {
            e.Property(p => p.Name).HasMaxLength(200).IsRequired();
            e.Property(p => p.Description).HasMaxLength(1000);
            e.Property(p => p.ImageUrl).HasMaxLength(500);
            e.Property(p => p.Price).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Cart>(e =>
        {
            e.HasMany(c => c.Items)
                .WithOne()
                .HasForeignKey(i => i.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CartItem>(e =>
        {
            e.HasIndex(i => new { i.CartId, i.ProductId }).IsUnique();
            e.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        SeedData.Apply(modelBuilder);
    }
}
