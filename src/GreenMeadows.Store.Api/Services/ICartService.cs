using GreenMeadows.Store.Api.Dtos;

namespace GreenMeadows.Store.Api.Services;

/// <summary>
/// All cart mutations live behind this service so controllers stay thin and the pricing /
/// stock rules have a single home that is easy to unit-test.
/// </summary>
public interface ICartService
{
    Task<CartDto> CreateCartAsync(CancellationToken cancellationToken = default);
    Task<CartDto> GetCartAsync(Guid cartId, CancellationToken cancellationToken = default);
    Task<CartDto> AddItemAsync(Guid cartId, Guid productId, int quantity, CancellationToken cancellationToken = default);
    Task<CartDto> UpdateItemQuantityAsync(Guid cartId, Guid productId, int quantity, CancellationToken cancellationToken = default);
    Task<CartDto> RemoveItemAsync(Guid cartId, Guid productId, CancellationToken cancellationToken = default);
}
