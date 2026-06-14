using System.ComponentModel.DataAnnotations;

namespace GreenMeadows.Store.Api.Dtos;

/// <summary>
/// Body for adding a product to a cart. Quantity is the amount to add to the existing line
/// </summary>
public record AddCartItemRequest
{
    [Required]
    public Guid ProductId { get; init; }

    [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000.")]
    public int Quantity { get; init; } = 1;
}
