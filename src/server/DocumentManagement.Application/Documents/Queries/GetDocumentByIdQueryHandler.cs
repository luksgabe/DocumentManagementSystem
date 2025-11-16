using DocumentManagement.Application.Documents.DTOs;
using DocumentManagement.Core.Exceptions;
using DocumentManagement.Core.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.Application.Documents.Queries;

public class GetDocumentByIdQueryHandler
    : IRequestHandler<GetDocumentByIdQuery, DocumentDto>
{
    private readonly IDocumentRepository _repository;
    private readonly ILogger<GetDocumentByIdQueryHandler> _logger;

    public GetDocumentByIdQueryHandler(
        IDocumentRepository repository,
        ILogger<GetDocumentByIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<DocumentDto> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting document {DocumentId}", request.Id);

        var document = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (document is null)
        {
            _logger.LogWarning("Document {DocumentId} not found", request.Id);
            throw new NotFoundException("Document not found");
        }

        return new DocumentDto
        (
            document.Id,
            document.Title,
            document.Description,
            document.AccessType,
            document.OwnerEmail,
            document.CreatedAt,
            document.UpdatedAt,
           document.DocumentTags.Select(dt => dt.Tag.Name).ToArray(),
            document.FileSizeBytes,
            document.ContentType
        );
    }
}