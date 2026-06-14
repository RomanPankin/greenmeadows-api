namespace GreenMeadows.Store.Api.Dtos;

/// <summary>
/// A cart plus its money breakdown. All totals are calculated on the server so the client
/// never has to trust or replicate pricing logic.
/// </summary>
public record CartDto(
    Guid Id,
    IReadOnlyList<CartItemDto> Items,
    decimal Subtotal,
    decimal TaxRate,
    decimal Tax,
    decimal Total,
    string Currency);
