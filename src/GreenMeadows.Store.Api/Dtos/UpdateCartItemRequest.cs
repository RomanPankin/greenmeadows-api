using System.ComponentModel.DataAnnotations;

namespace GreenMeadows.Store.Api.Dtos;

/// <summary>
/// Body for setting the absolute quantity of an existing cart line
/// </summary>
public record UpdateCartItemRequest
{
    [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000.")]
    public int Quantity { get; init; }
}
