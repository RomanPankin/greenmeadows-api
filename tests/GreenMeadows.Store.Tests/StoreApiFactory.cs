using GreenMeadows.Store.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GreenMeadows.Store.Tests;

/// <summary>
/// Boots the real API in-process but swaps Postgres for an in-memory Sqlite database so the
/// HTTP-level tests run with no external dependencies. The startup schema creation
/// (EnsureCreated for non-Npgsql providers) seeds the catalogue automatically.
/// </summary>
public class StoreApiFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection = TestDbContextFactory.CreateOpenConnection();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        // Pin pricing so the HTTP tests are deterministic and independent of appsettings.json,
        // which can change. These overrides win over any file-based config.
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Pricing:TaxRate"] = "0.20",
                ["Pricing:Currency"] = "GBP",
            });
        });

        builder.ConfigureServices(services =>
        {
            // Drop the Npgsql registration and point the context at the shared Sqlite connection.
            services.RemoveAll<DbContextOptions<StoreDbContext>>();
            services.AddDbContext<StoreDbContext>(options => options.UseSqlite(_connection));
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _connection.Dispose();
        }
    }
}
