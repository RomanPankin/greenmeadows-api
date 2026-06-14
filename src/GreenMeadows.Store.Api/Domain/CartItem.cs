namespace GreenMeadows.Store.Api.Domain;

/// <summary>
/// A line in a cart: a product and how many of it. Unique per (Cart, Product)
/// </summary>
public class CartItem
{
    public Guid Id { get; set; }

    public Guid CartId { get; set; }

    public Guid ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }
}
