namespace GreenMeadows.Store.Api.Dtos;

/// <summary>
/// A single cart line, with the line total pre-computed server-side
/// </summary>
public record CartItemDto(
    Guid ProductId,
    string Name,
    string? ImageUrl,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal);
