using GreenMeadows.Store.Api.Data;
using GreenMeadows.Store.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GreenMeadows.Store.Api.Services;

public class ProductService(StoreDbContext db) : IProductService
{
    public async Task<IReadOnlyList<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // Inline projection so EF translates it to a SELECT of just these columns.
        return await db.Products
            .OrderBy(p => p.Name)
            .Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price, p.ImageUrl, p.StockQuantity))
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await db.Products.FindAsync([id], cancellationToken);
        return product is null ? null : ProductDto.FromEntity(product);
    }
}
