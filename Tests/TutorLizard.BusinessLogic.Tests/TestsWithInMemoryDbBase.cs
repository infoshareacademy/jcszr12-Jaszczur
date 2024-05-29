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
    private JaszczurContext SetupInMemoryDbContext()
    {
        DbContextOptionsBuilder<JaszczurContext> dbBuilder = new();
        dbBuilder.UseInMemoryDatabase(databaseName: $"FakeDb{Guid.NewGuid()}");
        JaszczurContext context = new(dbBuilder.Options);
        return context;
    }
}
