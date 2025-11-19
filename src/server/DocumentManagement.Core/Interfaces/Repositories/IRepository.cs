using System.Linq.Expressions;

namespace DocumentManagement.Core.Interfaces.Repositories;

public interface IRepository<TEntity> : IDisposable where TEntity : class
{
    Task AddAsync(TEntity obj, CancellationToken cancellationToken);
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<TEntity>> GetCustomData(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);
    Task UpdateAsync(TEntity obj, CancellationToken cancellationToken);
    Task RemoveAsync(TEntity entity, CancellationToken cancellationToken);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
