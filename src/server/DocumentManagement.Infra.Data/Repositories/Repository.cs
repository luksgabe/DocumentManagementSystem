using DocumentManagement.Core.Interfaces.Repositories;
using DocumentManagement.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DocumentManagement.Infra.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
{
    protected AppDbContext _context { get; }

    public Repository(AppDbContext context)
    {
        _context = context;
        _context.ChangeTracker.LazyLoadingEnabled = false;
    }

    private DbSet<TEntity> _dbSet => _context.Set<TEntity>();

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetCustomData(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().Where(expression).ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync(id, cancellationToken);
    }

    public virtual async Task AddAsync(TEntity obj, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(obj, cancellationToken);
    }

    public virtual async Task UpdateAsync(TEntity obj, CancellationToken cancellationToken)
    {
        await Task.Run(() => _dbSet.Update(obj));
    }

    public virtual async Task RemoveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await Task.FromResult(_dbSet.Remove(entity));
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
