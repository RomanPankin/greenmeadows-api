namespace GreenMeadows.Store.Api.Domain;

/// <summary>
/// A server-side shopping cart. Identified by an opaque GUID the client persists locally
/// </summary>
public class Cart
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public List<CartItem> Items { get; set; } = [];
}
