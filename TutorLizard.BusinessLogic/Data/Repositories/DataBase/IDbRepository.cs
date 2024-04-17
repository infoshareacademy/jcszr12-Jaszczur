
namespace TutorLizard.BusinessLogic.Data.Repositories.DataBase;

public interface IDbRepository<TEntity> where TEntity : class
{
    Task<TEntity> Create(TEntity entity);
    Task<TEntity?> Delete<VId>(VId id);
    IQueryable<TEntity> GetAll();
    Task<TEntity?> GetById<VId>(VId id);
    Task<TEntity?> Update<VId>(VId id, Action<TEntity> update);
}