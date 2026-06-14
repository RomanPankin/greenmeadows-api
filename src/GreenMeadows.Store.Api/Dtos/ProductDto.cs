using GreenMeadows.Store.Api.Domain;

namespace GreenMeadows.Store.Api.Dtos;

/// <summary>
/// A catalogue product as returned by the API. Mirrors the entity but is a separate contract
/// so persistence concerns never leak to clients.
/// </summary>
public record ProductDto(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    string? ImageUrl,
    int StockQuantity)
{
    public static ProductDto FromEntity(Product p)
    {
        return new ProductDto(p.Id, p.Name, p.Description, p.Price, p.ImageUrl, p.StockQuantity);
    }
}
