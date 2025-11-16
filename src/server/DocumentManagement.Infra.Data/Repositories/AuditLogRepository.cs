using DocumentManagement.Core.Entities;
using DocumentManagement.Core.Interfaces.Repositories;
using DocumentManagement.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Infra.Data.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly AppDbContext _context;

    public AuditLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AuditLog log, CancellationToken ct = default)
    {
        await _context.AuditLogs.AddAsync(log, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<AuditLog>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
    {
        return await _context.AuditLogs
            .OrderByDescending(x => x.WhenUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }
}