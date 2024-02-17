using Core.Domain;
using Core.Domain.Context;
using Core.Domain.Repository;
using Persistence.Repository;

namespace Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly CoreContext _dbContext;
    private bool _disposed;
    private Dictionary<string, object?>? _repos;

    public UnitOfWork(CoreContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IGenericRepository<TRepository> GenericRepository<TRepository>() where TRepository : class
    {
        _repos ??= new Dictionary<string, object?>();

        var type = typeof(TRepository).Name;
        if (!_repos.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<TRepository>);
            var repositoryInstance = Activator.CreateInstance(repositoryType, _dbContext);
            _repos.Add(type, repositoryInstance);
        }

        return _repos[type] as GenericRepository<TRepository> ??
               throw new InvalidOperationException($"Repository: {type} - not loaded");
    }

    public void CreateTransaction()
    {
        _dbContext.Database.BeginTransaction();
    }

    public async Task CreateTransactionAsync()
    {
        await _dbContext.Database.BeginTransactionAsync();
    }

    public void Commit()
    {
        _dbContext.Database.CommitTransaction();
    }


    public async Task CommitAsync()
    {
        await _dbContext.Database.CommitTransactionAsync();
    }

    public void Rollback()
    {
        _dbContext.Database.RollbackTransaction();
        _dbContext.Dispose();
    }

    public async Task RollbackAsync()
    {
        await _dbContext.Database.RollbackTransactionAsync();
        await _dbContext.DisposeAsync();
    }


    public int Save()
    {
        return _dbContext.SaveChanges();
    }

    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing) _dbContext.Dispose();

        _disposed = true;
    }
}