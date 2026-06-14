using Asp.Versioning;
using GreenMeadows.Store.Api.Dtos;
using GreenMeadows.Store.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GreenMeadows.Store.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/products")]
[Produces("application/json")]
public class ProductsController(IProductService products) : ControllerBase
{
    /// <summary>Returns the full product catalogue.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await products.GetAllAsync(cancellationToken));
    }

    /// <summary>Returns a single product by id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await products.GetByIdAsync(id, cancellationToken);
        
        return product is null ? NotFound() : Ok(product);
    }
}
