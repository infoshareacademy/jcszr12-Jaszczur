using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Data.Repositories.DataBase;

namespace TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
public class DbRepository<TEntity, UDbContext> : IDbRepository<TEntity>
    where TEntity : class
    where UDbContext : DbContext
{
    private readonly UDbContext _dbContext;

    public DbRepository(UDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbContext.Set<TEntity>()
            .AsQueryable();
    }

    public async Task<TEntity?> GetById<VId>(VId id)
    {
        return await _dbContext.Set<TEntity>()
            .FindAsync(id);
    }

    public async Task<TEntity> Create(TEntity entity)
    {
        _dbContext
            .Set<TEntity>()
            .Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity?> Update<VId>(VId id, Action<TEntity> update)
    {
        TEntity? toUpdate = await GetById(id);
        if (toUpdate is null)
            return null;

        update.Invoke(toUpdate);
        await _dbContext.SaveChangesAsync();

        return toUpdate;
    }

    public async Task<TEntity?> Delete<VId>(VId id)
    {
        TEntity? toDelete = await GetById(id);
        if (toDelete is null)
            return null;

        _dbContext.Set<TEntity>()
            .Remove(toDelete);
        await _dbContext.SaveChangesAsync();

        return toDelete;
    }
}
