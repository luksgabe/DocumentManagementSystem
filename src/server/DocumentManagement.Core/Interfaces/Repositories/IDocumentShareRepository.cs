using DocumentManagement.Core.Entities;

namespace DocumentManagement.Core.Interfaces.Repositories;

public interface IDocumentShareRepository : IRepository<DocumentShare>
{
    Task<IEnumerable<DocumentShare>> GetByDocumentId(Guid documentId, CancellationToken cancellationToken);
}
