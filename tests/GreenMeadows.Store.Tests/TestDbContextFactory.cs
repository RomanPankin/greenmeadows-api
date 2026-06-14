using GreenMeadows.Store.Api.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GreenMeadows.Store.Tests;

/// <summary>
/// Builds a StoreDbContext backed by an in-memory Sqlite database. Sqlite (rather than the EF
/// in-memory provider) is used so relational behaviour the cart logic relies on — unique
/// indexes, FK constraints, real SQL — is actually exercised. The connection is kept open by
/// the caller for the lifetime of the database.
/// </summary>
public static class TestDbContextFactory
{
    public static SqliteConnection CreateOpenConnection()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        return connection;
    }

    public static StoreDbContext Create(SqliteConnection connection)
    {
        var options = new DbContextOptionsBuilder<StoreDbContext>()
            .UseSqlite(connection)
            .Options;

        var db = new StoreDbContext(options);
        db.Database.EnsureCreated(); // creates schema + applies HasData seed
        return db;
    }
}
