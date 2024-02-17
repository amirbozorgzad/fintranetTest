using System.Linq.Expressions;

namespace Core.Domain.Repository;

public interface IGenericRepository<TEntity> where TEntity : class
{
    void Insert(TEntity entity);
    Task InsertAsync(TEntity entity);
    void InsertRange(IEnumerable<TEntity> entities);
    Task InsertRangeAsync(IEnumerable<TEntity> entities);

    IQueryable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");

    TEntity? GetByID(object id);
    Task<TEntity?> GetByIDAsync(object id);
    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);
    void Delete(object id);
    void Delete(TEntity entity);
    void DeleteRange(IEnumerable<TEntity> entities);
}