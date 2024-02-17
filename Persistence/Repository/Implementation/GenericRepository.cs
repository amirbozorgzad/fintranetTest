using System.Linq.Expressions;
using Core.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(DbContext dbContext)
    {
        _dbSet = dbContext.Set<TEntity>();
    }

    public void Insert(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public async Task InsertAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void InsertRange(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }

    public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public IQueryable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null) query = query.Where(filter);

        query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        return orderBy != null ? orderBy(query) : query;
    }

    public TEntity? GetByID(object id)
    {
        return _dbSet.Find(id);
    }

    public async Task<TEntity?> GetByIDAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbSet;
    }

    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
    {
        return _dbSet.Where(expression);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public void Delete(object id)
    {
        var entityToDelete = _dbSet.Find(id);
        if (entityToDelete != null)
            _dbSet.Remove(entityToDelete);
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}