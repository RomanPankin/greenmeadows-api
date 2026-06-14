using GreenMeadows.Store.Api.Data;
using GreenMeadows.Store.Api.Domain;
using GreenMeadows.Store.Api.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GreenMeadows.Store.Api.Services;

public class CartService(StoreDbContext db, IOptions<PricingOptions> pricing, TimeProvider clock) : ICartService
{
    private readonly PricingOptions _pricing = pricing.Value;

    public async Task<CartDto> CreateCartAsync(CancellationToken cancellationToken = default)
    {
        var now = clock.GetUtcNow();
        var cart = new Cart { Id = Guid.NewGuid(), CreatedAt = now, UpdatedAt = now };
        db.Carts.Add(cart);
        await db.SaveChangesAsync(cancellationToken);
        return CartMapper.ToDto(cart, _pricing);
    }

    public async Task<CartDto> GetCartAsync(Guid cartId, CancellationToken cancellationToken = default)
    {
        var cart = await LoadCartAsync(cartId, cancellationToken);
        return CartMapper.ToDto(cart, _pricing);
    }

    public async Task<CartDto> AddItemAsync(Guid cartId, Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var cart = await LoadCartAsync(cartId, cancellationToken);
        var product = await db.Products.FindAsync([productId], cancellationToken)
            ?? throw new NotFoundException($"Product '{productId}' was not found.");

        var line = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        var newQuantity = (line?.Quantity ?? 0) + quantity;
        EnsureStock(product, newQuantity);

        if (line is null)
        {
            // No Id is set: a default key tells EF this is a new row to INSERT. Setting one would
            // make EF treat the entity (discovered via the tracked cart) as an existing row to UPDATE.
            cart.Items.Add(new CartItem { CartId = cart.Id, ProductId = productId, Quantity = quantity });
        }
        else
        {
            line.Quantity = newQuantity;
        }

        await TouchAndSaveAsync(cart, cancellationToken);
        return CartMapper.ToDto(cart, _pricing);
    }

    public async Task<CartDto> UpdateItemQuantityAsync(Guid cartId, Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var cart = await LoadCartAsync(cartId, cancellationToken);
        var line = cart.Items.FirstOrDefault(i => i.ProductId == productId)
            ?? throw new NotFoundException($"Product '{productId}' is not in this cart.");

        EnsureStock(line.Product, quantity);
        line.Quantity = quantity;

        await TouchAndSaveAsync(cart, cancellationToken);
        return CartMapper.ToDto(cart, _pricing);
    }

    public async Task<CartDto> RemoveItemAsync(Guid cartId, Guid productId, CancellationToken cancellationToken = default)
    {
        var cart = await LoadCartAsync(cartId, cancellationToken);
        var line = cart.Items.FirstOrDefault(i => i.ProductId == productId)
            ?? throw new NotFoundException($"Product '{productId}' is not in this cart.");

        cart.Items.Remove(line);
        db.CartItems.Remove(line);

        await TouchAndSaveAsync(cart, cancellationToken);
        return CartMapper.ToDto(cart, _pricing);
    }

    private async Task<Cart> LoadCartAsync(Guid cartId, CancellationToken cancellationToken)
    {
        return await db.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.Id == cartId, cancellationToken)
            ?? throw new NotFoundException($"Cart '{cartId}' was not found.");
    }

    private static void EnsureStock(Product product, int requestedQuantity)
    {
        if (requestedQuantity > product.StockQuantity)
        {
            throw new BusinessRuleException(
                $"Only {product.StockQuantity} of '{product.Name}' in stock; requested {requestedQuantity}.");
        }
    }

    private async Task TouchAndSaveAsync(Cart cart, CancellationToken cancellationToken)
    {
        cart.UpdatedAt = clock.GetUtcNow();
        await db.SaveChangesAsync(cancellationToken);
    }
}
