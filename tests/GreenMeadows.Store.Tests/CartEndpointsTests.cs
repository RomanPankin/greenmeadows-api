using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GreenMeadows.Store.Api.Dtos;

namespace GreenMeadows.Store.Tests;

/// <summary>
/// HTTP-level tests over the real pipeline: routing, versioning, model validation, status codes
/// and ProblemDetails. They cover the contract the frontend depends on.
/// </summary>
public class CartEndpointsTests(StoreApiFactory factory) : IClassFixture<StoreApiFactory>
{
    private static readonly Guid SpadeId = new("11111111-1111-1111-1111-111111111111");
    private static readonly Guid ShearsId = new("55555555-5555-5555-5555-555555555555"); // stock 3

    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetProducts_returns_seeded_catalogue()
    {
        var products = await _client.GetFromJsonAsync<List<ProductDto>>("/api/v1/products");

        products.Should().NotBeNull();
        products!.Should().HaveCountGreaterThan(0);
        products.Should().Contain(p => p.Id == SpadeId);
    }

    [Fact]
    public async Task GetProduct_unknown_id_returns_404()
    {
        var response = await _client.GetAsync($"/api/v1/products/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateCart_returns_201_with_location_and_empty_cart()
    {
        var response = await _client.PostAsync("/api/v1/carts", null);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        var cart = await response.Content.ReadFromJsonAsync<CartDto>();
        cart!.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task AddItem_then_get_returns_persisted_cart()
    {
        var cartId = await CreateCartAsync();

        var add = await _client.PostAsJsonAsync($"/api/v1/carts/{cartId}/items",
            new AddCartItemRequest { ProductId = SpadeId, Quantity = 2 });
        add.StatusCode.Should().Be(HttpStatusCode.OK);

        var cart = await _client.GetFromJsonAsync<CartDto>($"/api/v1/carts/{cartId}");
        cart!.Items.Should().ContainSingle(i => i.ProductId == SpadeId && i.Quantity == 2);
        cart.Subtotal.Should().Be(49.98m);
        cart.Total.Should().Be(59.98m);
    }

    [Fact]
    public async Task AddItem_beyond_stock_returns_409_problem_details()
    {
        var cartId = await CreateCartAsync();

        var response = await _client.PostAsJsonAsync($"/api/v1/carts/{cartId}/items",
            new AddCartItemRequest { ProductId = ShearsId, Quantity = 9 });

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var problem = await response.Content.ReadFromJsonAsync<ProblemPayload>();
        problem!.Status.Should().Be(409);
        problem.Detail.Should().Contain("stock");
    }

    [Fact]
    public async Task AddItem_with_invalid_quantity_returns_400()
    {
        var cartId = await CreateCartAsync();

        var response = await _client.PostAsJsonAsync($"/api/v1/carts/{cartId}/items",
            new AddCartItemRequest { ProductId = SpadeId, Quantity = 0 });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetCart_unknown_id_returns_404()
    {
        var response = await _client.GetAsync($"/api/v1/carts/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RemoveItem_empties_the_cart()
    {
        var cartId = await CreateCartAsync();
        await _client.PostAsJsonAsync($"/api/v1/carts/{cartId}/items",
            new AddCartItemRequest { ProductId = SpadeId, Quantity = 1 });

        var response = await _client.DeleteAsync($"/api/v1/carts/{cartId}/items/{SpadeId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var cart = await response.Content.ReadFromJsonAsync<CartDto>();
        cart!.Items.Should().BeEmpty();
    }

    private async Task<Guid> CreateCartAsync()
    {
        var response = await _client.PostAsync("/api/v1/carts", null);
        var cart = await response.Content.ReadFromJsonAsync<CartDto>();
        return cart!.Id;
    }

    private record ProblemPayload(int Status, string? Detail);
}
