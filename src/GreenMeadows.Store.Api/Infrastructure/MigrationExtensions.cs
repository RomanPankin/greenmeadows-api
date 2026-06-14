using GreenMeadows.Store.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace GreenMeadows.Store.Api.Infrastructure;

public static class MigrationExtensions
{
    /// <summary>
    /// Brings the database schema up to date at startup. Against Postgres it runs EF migrations;
    /// against any other provider (the Sqlite used in integration tests) it just creates the
    /// schema from the model, seed data included.
    /// </summary>
    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

        if (db.Database.IsNpgsql())
        {
            await db.Database.MigrateAsync();
        }
        else
        {
            await db.Database.EnsureCreatedAsync();
        }
    }
}
