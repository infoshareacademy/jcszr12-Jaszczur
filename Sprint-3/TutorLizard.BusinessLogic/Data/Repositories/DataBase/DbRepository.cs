using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TutorLizard.BusinessLogic.Extensions;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;

namespace TutorLizard.BusinessLogic.Data.Repositories.DataBase;
public class DbRepository<TEntity, UDbContext> : IDbRepository<TEntity>
    where TEntity : class
    where UDbContext : DbContext
{
    private readonly UDbContext _dbContext;
    private readonly ILogger<DbRepository<TEntity, UDbContext>> _logger;

    public DbRepository(UDbContext dbContext,
                        ILogger<DbRepository<TEntity, UDbContext>> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbContext.Set<TEntity>()
            .AsQueryable();
    }

    public async Task<TEntity?> GetById<VId>(VId id)
    {
        TEntity? entity = await _dbContext
            .Set<TEntity>()
            .FindAsync(id);

        if (entity is null)
        {
            _logger.LogEntityNotFound(nameof(GetById), id);
        }
        else
        {
            _logger.LogEntityFound(nameof(GetById), id);
        }

        return entity;
    }

    public async Task<TEntity> Create(TEntity entity)
    {
        _dbContext
            .Set<TEntity>()
            .Add(entity);
        await _dbContext.SaveChangesAsync();

        _logger.LogEntityCreated(nameof(Create), GetEntityId(entity));

        return entity;
    }

    public async Task<TEntity?> Update<VId>(VId id, Action<TEntity> updateAction)
    {
        TEntity? toUpdate = await GetById(id);
        if (toUpdate is null)
        {
            _logger.LogEntityNotUpdated(nameof(Update), id);
            return null;
        }

        updateAction.Invoke(toUpdate);
        await _dbContext.SaveChangesAsync();

        _logger.LogEntityUpdated(nameof(Update), id);

        return toUpdate;
    }

    public async Task<TEntity?> Delete<VId>(VId id)
    {
        TEntity? toDelete = await GetById(id);
        if (toDelete is null)
        {
            _logger.LogEntityNotDeleted(nameof(Delete), id);
            return null;
        }

        _dbContext.Set<TEntity>()
            .Remove(toDelete);
        await _dbContext.SaveChangesAsync();

        _logger.LogEntityDeleted(nameof(Delete), id);

        return toDelete;
    }

    private object? GetEntityId(TEntity entity)
    {
        var entry = _dbContext.Entry(entity);
        var id = entry.Metadata.FindPrimaryKey()?
            .Properties
            .Select(p => entry.Property(p.Name).CurrentValue)
            .FirstOrDefault();

        return id;
    }
}
