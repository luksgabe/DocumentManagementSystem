using DocumentManagement.Core.Entities;

namespace DocumentManagement.Core.Interfaces.Repositories;

public interface IDocumentRepository : IRepository<Document>
{

    Task AddDocumentAsync(Document document, IEnumerable<string> tags, CancellationToken ct = default);
    Task UpdateDocumentAsync(Document document, IEnumerable<string> tags, CancellationToken ct = default);
    Task<(IReadOnlyList<Document> Items, int TotalCount)> GetPagedAsync(
    string? search,
    string? tag,
    string? contentType,
    int page,
    int pageSize,
    CancellationToken ct = default);
}
