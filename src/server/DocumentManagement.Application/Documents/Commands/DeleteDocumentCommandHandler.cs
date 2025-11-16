using DocumentManagement.Core.Entities;
using DocumentManagement.Core.Exceptions;
using DocumentManagement.Core.Interfaces.Repositories;
using DocumentManagement.Core.Interfaces.Services;
using DocumentManagement.Core.Interfaces.Storage;
using MediatR;
using Microsoft.Extensions.Logging;


namespace DocumentManagement.Application.Documents.Commands;

public class DeleteDocumentCommandHandler
    : IRequestHandler<DeleteDocumentCommand, Unit>
{
    private readonly IDocumentRepository _repository;
    private readonly IAuditService _auditService;
    private readonly IFileStorage _fileStorage;
    private readonly ILogger<DeleteDocumentCommandHandler> _logger;

    public DeleteDocumentCommandHandler(
        IDocumentRepository repository,
        IFileStorage fileStorage,
        IAuditService auditService,
        ILogger<DeleteDocumentCommandHandler> logger)
    {
        _repository = repository;
        _fileStorage = fileStorage;
        _auditService = auditService;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting document {DocumentId}", request.Id);


        var document = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (document is null)
        {
            _logger.LogWarning("Document {DocumentId} not found for delete", request.Id);
            throw new NotFoundException("Document not found");
        }

        await _repository.RemoveAsync(document, cancellationToken);
        await _fileStorage.RemoveAsync(document.StorageUri, cancellationToken);

        _logger.LogInformation("Document {DocumentId} deleted", request.Id);

        await _auditService.LogAsync(document.OwnerSub, document.OwnerEmail,
         "DocumentDeleted", nameof(Document), document.Id.ToString());

        return Unit.Value;
    }
}