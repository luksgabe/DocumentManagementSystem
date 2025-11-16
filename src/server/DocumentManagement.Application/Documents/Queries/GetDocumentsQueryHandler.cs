using DocumentManagement.Application.Documents.DTOs;
using DocumentManagement.Core.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.Application.Documents.Queries;

public class GetDocumentsQueryHandler
    : IRequestHandler<GetDocumentsQuery, DocumentListResultDto>
{
    private readonly IDocumentRepository _repository;
    private readonly ILogger<GetDocumentsQueryHandler> _logger;

    public GetDocumentsQueryHandler(
        IDocumentRepository repository,
        ILogger<GetDocumentsQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<DocumentListResultDto> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listing documents. Search={Search}, Tag={Tag}, ContentType={ContentType}, Page={Page}, PageSize={PageSize}",
            request.Search, request.Tag, request.ContentType, request.Page, request.PageSize);

        var (items, total) = await _repository.GetPagedAsync(
            request.Search,
            request.Tag,
            request.ContentType,
            request.Page,
            request.PageSize,
            cancellationToken);

        var docs = items.Select(d => new DocumentDto
        (
            d.Id,
            d.Title,
            d.Description,
            d.AccessType,
            d.OwnerEmail,
            d.CreatedAt,
            d.UpdatedAt,
            d.DocumentTags.Select(dt => dt.Tag.Name).ToArray(),
            d.FileSizeBytes,
            d.ContentType
        )).ToList();

        return new DocumentListResultDto
        {
            Items = docs,
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}