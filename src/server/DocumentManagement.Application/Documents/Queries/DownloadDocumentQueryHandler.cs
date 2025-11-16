using DocumentManagement.Application.Documents.DTOs;
using DocumentManagement.Core.Exceptions;
using DocumentManagement.Core.Interfaces.Repositories;
using DocumentManagement.Core.Interfaces.Services;
using DocumentManagement.Core.Interfaces.Storage;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.Application.Documents.Queries;

public class DownloadDocumentQueryHandler
    : IRequestHandler<DownloadDocumentQuery, DocumentDownloadDto>
{
    private readonly IDocumentRepository _repository;
    private readonly IFileStorage _fileStorage;
    private readonly IAuditService _auditService;
    private readonly ILogger<DownloadDocumentQueryHandler> _logger;

    public DownloadDocumentQueryHandler(
        IDocumentRepository repository,
        IFileStorage fileStorage,
        IAuditService auditService,
        ILogger<DownloadDocumentQueryHandler> logger)
    {
        _repository = repository;
        _auditService = auditService;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<DocumentDownloadDto> Handle(DownloadDocumentQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Downloading document {DocumentId}", request.Id);

        var document = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (document is null)
        {
            _logger.LogWarning("Document {DocumentId} not found for download", request.Id);
            throw new NotFoundException("Document not found");
        }

        var stream = await _fileStorage.OpenReadAsync(document.StorageUri, cancellationToken);
        
        await _auditService.LogAsync(document.OwnerSub, document.OwnerEmail,
            "DocumentDownloaded", "Document", document.Id.ToString(),
            new { FileName = document.FileName });

        return new DocumentDownloadDto(stream, document.FileName, document.ContentType);
    }
}