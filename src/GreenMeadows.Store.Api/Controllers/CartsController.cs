using Asp.Versioning;
using GreenMeadows.Store.Api.Dtos;
using GreenMeadows.Store.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GreenMeadows.Store.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/carts")]
[Produces("application/json")]
public class CartsController(ICartService carts) : ControllerBase
{
    /// <summary>
    /// Creates a new empty cart
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<CartDto>> Create(CancellationToken cancellationToken)
    {
        var cart = await carts.CreateCartAsync(cancellationToken);
        
        return CreatedAtAction(nameof(GetById), new { id = cart.Id }, cart);
    }

    /// <summary>
    /// Returns the cart details by id
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CartDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        return Ok(await carts.GetCartAsync(id, cancellationToken));
    }

    /// <summary>
    /// Adds a product to the cart (or increases its quantity if already present)
    /// </summary>
    [HttpPost("{id:guid}/items")]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CartDto>> AddItem(Guid id, AddCartItemRequest request, CancellationToken cancellationToken)
    {
        return Ok(await carts.AddItemAsync(id, request.ProductId, request.Quantity, cancellationToken));
    }

    /// <summary>
    /// Sets the absolute quantity of a line in the cart
    /// </summary>
    [HttpPatch("{id:guid}/items/{productId:guid}")]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CartDto>> UpdateItem(Guid id, Guid productId, UpdateCartItemRequest request, CancellationToken cancellationToken)
    {
        return Ok(await carts.UpdateItemQuantityAsync(id, productId, request.Quantity, cancellationToken));
    }

    /// <summary>Removes a line from the cart.</summary>
    [HttpDelete("{id:guid}/items/{productId:guid}")]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CartDto>> RemoveItem(Guid id, Guid productId, CancellationToken cancellationToken)
    {
        return Ok(await carts.RemoveItemAsync(id, productId, cancellationToken));
    }
}
