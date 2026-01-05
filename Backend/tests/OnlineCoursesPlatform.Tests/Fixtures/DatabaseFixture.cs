using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Infrastructure.Data;
using Xunit;

namespace OnlineCoursesPlatform.Tests.Fixtures;

/// <summary>
/// Fixture para proporcionar una base de datos en memoria para tests.
/// </summary>
public class DatabaseFixture : IDisposable
{
    public ApplicationDbContext Context { get; private set; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new ApplicationDbContext(options);
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}

/// <summary>
/// Collection definition para compartir el fixture entre tests.
/// </summary>
[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
}
