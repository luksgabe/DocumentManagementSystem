using DocumentManagement.Application.Documents.DTOs;
using DocumentManagement.Core.Interfaces.Repositories;
using DocumentManagement.Core.Interfaces.Storage;
using MediatR;
using Microsoft.Extensions.Logging;
using DocumentManagement.Core.Entities;
using DocumentManagement.Core.Interfaces.Services;
using DocumentManagement.Core.Services;

namespace DocumentManagement.Application.Documents.Commands;

public  class CreateDocumentCommandHandler
    : IRequestHandler<CreateDocumentCommand, DocumentDto>
{
    private readonly IDocumentRepository _repository;
    private readonly IAuditService _auditService;
    private readonly IFileStorage _fileStorage;
    private readonly ILogger<CreateDocumentCommandHandler> _logger;

    public CreateDocumentCommandHandler(
        IDocumentRepository repository,
        IFileStorage fileStorage,
        IAuditService auditService,
        ILogger<CreateDocumentCommandHandler> logger)
    {
        _repository = repository;
        _fileStorage = fileStorage;
        _auditService = auditService;
        _logger = logger;
    }

    public async Task<DocumentDto> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating document '{Title}' for {OwnerEmail}", request.Title, request.OwnerEmail);
        

        await using var stream = request.File.OpenReadStream();
        var storageUri = await _fileStorage.SaveAsync(stream, request.File.FileName, request.File.ContentType, cancellationToken);

        var document = new Document
        (
           Guid.NewGuid(),
           request.Title,
           request.Description!,
           request.AccessType,
            request.File.FileName,
            request.File.Length,
           request.File.ContentType,
            request.OwnerSub,
            request.OwnerEmail,
            storageUri
        );

        await _repository.AddDocumentAsync(document, request.Tags, cancellationToken);

        _logger.LogInformation("Document {DocumentId} created successfully", document.Id);
        
        await _auditService.LogAsync(request.OwnerSub, request.OwnerEmail,
         "DocumentCreated", nameof(Document), document.Id.ToString());

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