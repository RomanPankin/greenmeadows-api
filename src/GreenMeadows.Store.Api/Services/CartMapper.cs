using GreenMeadows.Store.Api.Domain;
using GreenMeadows.Store.Api.Dtos;

namespace GreenMeadows.Store.Api.Services;

/// <summary>Turns a cart entity into a DTO and computes the money breakdown in one place.</summary>
public static class CartMapper
{
    public static CartDto ToDto(Cart cart, PricingOptions pricing)
    {
        var items = cart.Items
            .OrderBy(i => i.Product.Name)
            .Select(i => new CartItemDto(
                i.ProductId,
                i.Product.Name,
                i.Product.ImageUrl,
                i.Product.Price,
                i.Quantity,
                Round(i.Product.Price * i.Quantity)))
            .ToList();

        var subtotal = Round(items.Sum(i => i.LineTotal));
        var tax = Round(subtotal * pricing.TaxRate);
        var total = Round(subtotal + tax);

        return new CartDto(cart.Id, items, subtotal, pricing.TaxRate, tax, total, pricing.Currency);
    }

    private static decimal Round(decimal value)
    {
        return Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}
