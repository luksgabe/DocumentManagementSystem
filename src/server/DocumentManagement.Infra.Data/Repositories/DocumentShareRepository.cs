using DocumentManagement.Core.Entities;
using DocumentManagement.Core.Interfaces.Repositories;
using DocumentManagement.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Infra.Data.Repositories;

public class DocumentShareRepository : Repository<DocumentShare>, IDocumentShareRepository
{
    public DocumentShareRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DocumentShare>> GetByDocumentId(Guid documentId, CancellationToken cancellationToken)
    {
        return await _context.DocumentShares.Where(ds => ds.DocumentId == documentId).ToListAsync(cancellationToken);
    }
}
