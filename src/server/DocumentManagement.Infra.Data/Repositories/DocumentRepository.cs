using DocumentManagement.Core.Entities;
using DocumentManagement.Core.Interfaces.Repositories;
using DocumentManagement.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DocumentManagement.Infra.Data.Repositories;

public class DocumentRepository : Repository<Document>, IDocumentRepository
{
    public DocumentRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Documents
            .AsNoTracking()
            .Include(d => d.DocumentTags)
                .ThenInclude(dt => dt.Tag)
            .Include(d => d.Shares)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task AddDocumentAsync(Document document, IEnumerable<string> tags, CancellationToken ct = default)
    {
        await AttachTagsAsync(document, tags, ct);

        await _context.Documents.AddAsync(document, ct);

        await AttachDocumentSharesAsync(document, ct);

        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateDocumentAsync(Document document, IEnumerable<string> tags, CancellationToken ct = default)
    {
        await AttachTagsAsync(document, tags, ct);

        await AttachDocumentSharesAsync(document, ct);


        _context.Documents.Attach(document);

        _context.Entry(document).State = EntityState.Modified;

        _context.Entry(document).Property(d => d.UpdatedAt).IsModified = true;

        await _context.SaveChangesAsync(ct);
    }

    public override async Task RemoveAsync(Document document, CancellationToken ct = default)
    {
        _context.Documents.Remove(document);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<(IReadOnlyList<Document> Items, int TotalCount)> GetPagedAsync(
        string? search,
        string? tag,
        string? contentType,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var query = _context.Documents
            .Include(d => d.DocumentTags)
                .ThenInclude(dt => dt.Tag)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();

            query = query.Where(d =>
                d.Title.ToLower().Contains(s) ||
                (d.Description != null && d.Description.ToLower().Contains(s)));
        }

        if (!string.IsNullOrWhiteSpace(tag))
        {
            var t = tag.Trim().ToLower();
            query = query.Where(d =>
                d.DocumentTags.Any(dt => dt.Tag.Name.ToLower() == t));
        }

        if (!string.IsNullOrWhiteSpace(contentType))
        {
            query = query.Where(d => d.ContentType == contentType);
        }

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(d => d.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    #region Private Methods

    private async Task AttachTagsAsync(Document document, IEnumerable<string> tags, CancellationToken ct)
    {
        var tagNames = tags
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Select(t => t.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (!tagNames.Any())
        {
            document.ClearDocumentsTag();
            return;
        }

        var existingTags = await _context.Tags
            .Where(t => tagNames.Contains(t.Name))
            .ToListAsync(ct);

        var newTagNames = tagNames
            .Except(existingTags.Select(t => t.Name), StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var newTagName in newTagNames)
        {
            var tag = new Tag(newTagName);
            await _context.Tags.AddAsync(tag, ct);
            existingTags.Add(tag);
        }

        document.ClearDocumentsTag();

        foreach (var tag in existingTags)
        {
            document.AddTag(tag);
        }
    }

    private async Task AttachDocumentSharesAsync(Document document, CancellationToken ct)
    {
        foreach (var share in document.Shares)
        {
            var entry = _context.Entry(share);

            if (entry.State == EntityState.Detached)
            {
                await _context.DocumentShares.AddAsync(share, ct);
            }
        }
    }

    #endregion
}
