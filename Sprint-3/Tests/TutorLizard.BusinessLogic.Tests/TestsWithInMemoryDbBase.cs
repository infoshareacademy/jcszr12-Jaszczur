using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Data;

namespace TutorLizard.BusinessLogic.Tests;
public abstract class TestsWithInMemoryDbBase : IDisposable
{
    protected JaszczurContext DbContext;

    protected TestsWithInMemoryDbBase()
    {
        DbContext = SetupInMemoryDbContext();
    }
    public void Dispose()
    {
        DbContext.Dispose();
    }

    protected IQueryable<TEntity> AddEntitiesToInMemoryDb<TEntity>(List<TEntity> entities)
    where TEntity : class
    {
        DbContext
            .Set<TEntity>()
            .AddRange(entities);
        DbContext.SaveChanges();

        return DbContext
            .Set<TEntity>()
            .AsQueryable();
    }

    private JaszczurContext SetupInMemoryDbContext()
    {
        DbContextOptionsBuilder<JaszczurContext> dbBuilder = new();
        dbBuilder.UseInMemoryDatabase(databaseName: $"FakeDb{Guid.NewGuid()}");
        JaszczurContext context = new(dbBuilder.Options);
        return context;
    }
}
