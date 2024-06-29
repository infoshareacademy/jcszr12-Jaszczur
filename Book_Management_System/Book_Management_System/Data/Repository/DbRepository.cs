using Microsoft.EntityFrameworkCore;
using Book_Management_System.Interfaces.Data;
using Book_Management_System.Data;

namespace TutorLizard.BusinessLogic.Data.Repositories.DataBase;
public class DbRepository : IDbRepository
{
    private readonly BMSContext _dbContext;

    public DbRepository(IDbContext dbContext)
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

    public async Task<TEntity?> Update<VId>(VId id, Action<TEntity> updateAction)
    {
        TEntity? toUpdate = await GetById(id);
        if (toUpdate is null)
            return null;

        updateAction.Invoke(toUpdate);
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
