using DocumentManagement.Core.Entities;

namespace DocumentManagement.Core.Interfaces.Repositories;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog log, CancellationToken ct = default);
    Task<IReadOnlyList<AuditLog>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
}
