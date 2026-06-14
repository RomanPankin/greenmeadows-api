using FluentAssertions;
using GreenMeadows.Store.Api.Data;
using GreenMeadows.Store.Api.Services;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace GreenMeadows.Store.Tests;

/// <summary>
/// Unit tests for the cart's business rules: line merging, quantity edits, stock enforcement,
/// removal, and money totals. These are the highest-value paths to protect — they hold the
/// pricing and inventory logic the rest of the app trusts.
/// </summary>
public class CartServiceTests : IDisposable
{
    private static readonly Guid AuroraId = new("11111111-1111-1111-1111-111111111111"); // Aurora X1, $899.00, stock 25
    private static readonly Guid EonMiniId = new("55555555-5555-5555-5555-555555555555"); // Eon Mini, $349.00, stock 3

    private readonly SqliteConnection _connection = TestDbContextFactory.CreateOpenConnection();
    private readonly StoreDbContext _db;
    private readonly CartService _service;

    public CartServiceTests()
    {
        _db = TestDbContextFactory.Create(_connection);
        var pricing = Options.Create(new PricingOptions { TaxRate = 0.20m, Currency = "NZD" });
        _service = new CartService(_db, pricing, TimeProvider.System);
    }

    [Fact]
    public async Task CreateCart_starts_empty_with_zero_totals()
    {
        var cart = await _service.CreateCartAsync();

        cart.Items.Should().BeEmpty();
        cart.Subtotal.Should().Be(0m);
        cart.Total.Should().Be(0m);
        cart.Currency.Should().Be("NZD");
    }

    [Fact]
    public async Task AddItem_adds_a_line_and_computes_totals()
    {
        var cart = await _service.CreateCartAsync();

        var result = await _service.AddItemAsync(cart.Id, AuroraId, 2);

        result.Items.Should().ContainSingle();
        var line = result.Items[0];
        line.Quantity.Should().Be(2);
        line.LineTotal.Should().Be(1798.00m);
        result.Subtotal.Should().Be(1798.00m);
        result.Tax.Should().Be(359.60m);   // 1798.00 * 0.20
        result.Total.Should().Be(2157.60m);
    }

    [Fact]
    public async Task AddItem_twice_merges_into_one_line()
    {
        var cart = await _service.CreateCartAsync();

        await _service.AddItemAsync(cart.Id, AuroraId, 2);
        var result = await _service.AddItemAsync(cart.Id, AuroraId, 1);

        result.Items.Should().ContainSingle();
        result.Items[0].Quantity.Should().Be(3);
    }

    [Fact]
    public async Task AddItem_beyond_stock_throws_and_persists_nothing()
    {
        var cart = await _service.CreateCartAsync();

        var act = () => _service.AddItemAsync(cart.Id, EonMiniId, 9); // stock is 3

        await act.Should().ThrowAsync<BusinessRuleException>();
        var reloaded = await _service.GetCartAsync(cart.Id);
        reloaded.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task AddItem_merge_that_exceeds_stock_throws()
    {
        var cart = await _service.CreateCartAsync();
        await _service.AddItemAsync(cart.Id, EonMiniId, 2); // ok, 2 <= 3

        var act = () => _service.AddItemAsync(cart.Id, EonMiniId, 2); // would be 4 > 3

        await act.Should().ThrowAsync<BusinessRuleException>();
    }

    [Fact]
    public async Task AddItem_to_unknown_cart_throws_NotFound()
    {
        var act = () => _service.AddItemAsync(Guid.NewGuid(), AuroraId, 1);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AddItem_with_unknown_product_throws_NotFound()
    {
        var cart = await _service.CreateCartAsync();

        var act = () => _service.AddItemAsync(cart.Id, Guid.NewGuid(), 1);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateItemQuantity_sets_absolute_quantity()
    {
        var cart = await _service.CreateCartAsync();
        await _service.AddItemAsync(cart.Id, AuroraId, 2);

        var result = await _service.UpdateItemQuantityAsync(cart.Id, AuroraId, 5);

        result.Items[0].Quantity.Should().Be(5);
        result.Subtotal.Should().Be(4495.00m);
    }

    [Fact]
    public async Task UpdateItemQuantity_beyond_stock_throws()
    {
        var cart = await _service.CreateCartAsync();
        await _service.AddItemAsync(cart.Id, EonMiniId, 1);

        var act = () => _service.UpdateItemQuantityAsync(cart.Id, EonMiniId, 10);

        await act.Should().ThrowAsync<BusinessRuleException>();
    }

    [Fact]
    public async Task UpdateItemQuantity_for_missing_line_throws_NotFound()
    {
        var cart = await _service.CreateCartAsync();

        var act = () => _service.UpdateItemQuantityAsync(cart.Id, AuroraId, 1);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task RemoveItem_deletes_the_line()
    {
        var cart = await _service.CreateCartAsync();
        await _service.AddItemAsync(cart.Id, AuroraId, 2);

        var result = await _service.RemoveItemAsync(cart.Id, AuroraId);

        result.Items.Should().BeEmpty();
        result.Subtotal.Should().Be(0m);
    }

    [Fact]
    public async Task RemoveItem_for_missing_line_throws_NotFound()
    {
        var cart = await _service.CreateCartAsync();

        var act = () => _service.RemoveItemAsync(cart.Id, AuroraId);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
