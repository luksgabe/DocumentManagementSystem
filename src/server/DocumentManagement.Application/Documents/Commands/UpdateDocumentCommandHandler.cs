using DocumentManagement.Application.Documents.DTOs;
using DocumentManagement.Core.Entities;
using DocumentManagement.Core.Exceptions;
using DocumentManagement.Core.Interfaces.Repositories;
using DocumentManagement.Core.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.Application.Documents.Commands;

public class UpdateDocumentCommandHandler
    : IRequestHandler<UpdateDocumentCommand, DocumentDto>
{
    private readonly IDocumentRepository _repository;
    private readonly IAuditService _auditService;
    private readonly ILogger<UpdateDocumentCommandHandler> _logger;

    public UpdateDocumentCommandHandler(
        IDocumentRepository repository,
        IAuditService auditService,
        ILogger<UpdateDocumentCommandHandler> logger)
    {
        _repository = repository;
        _auditService = auditService;
        _logger = logger;
    }

    public async Task<DocumentDto> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating document {DocumentId}", request.Id);

        var oldDocument = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (oldDocument is null)
        {
            _logger.LogWarning("Document {DocumentId} not found for update", request.Id);
            throw new NotFoundException("Document not found");
        }

        var newDocument = new Document(
        oldDocument.Id,
        request.Title,
        request.Description!,
        request.AccessType,
        oldDocument.FileName,
        oldDocument.FileSizeBytes,
        oldDocument.ContentType,
        oldDocument.OwnerSub,
        oldDocument.OwnerEmail,
        oldDocument.StorageUri);

        newDocument.Updated();

        await _repository.UpdateDocumentAsync(newDocument, request.Tags, cancellationToken);

        await _auditService.LogAsync(newDocument.OwnerSub, newDocument.OwnerEmail,
         "DocumentUpdated", nameof(Document), newDocument.Id.ToString());

        return new DocumentDto
        (
            newDocument.Id,
            newDocument.Title,
            newDocument.Description,
            newDocument.AccessType,
            newDocument.OwnerEmail,
            newDocument.CreatedAt,
            newDocument.UpdatedAt,
            newDocument.DocumentTags.Select(dt => dt.Tag.Name).ToArray(),
            newDocument.FileSizeBytes,
            newDocument.ContentType
        );
    }
}