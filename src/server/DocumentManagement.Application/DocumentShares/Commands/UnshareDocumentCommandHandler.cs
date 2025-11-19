using DocumentManagement.Core.Entities;
using DocumentManagement.Core.Exceptions;
using DocumentManagement.Core.Interfaces.Repositories;
using DocumentManagement.Core.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.Application.DocumentShares.Commands;

public class UnshareDocumentCommandHandler
    : IRequestHandler<UnshareDocumentCommand, Unit>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IDocumentShareRepository _documentShareRepository;
    private readonly IAuditService _auditService;
    private readonly ILogger<UnshareDocumentCommandHandler> _logger;

    public UnshareDocumentCommandHandler(
        IDocumentRepository documentRepository,
        IDocumentShareRepository documentShareRepository,
    IAuditService auditService,
        ILogger<UnshareDocumentCommandHandler> logger)
    {
        _documentRepository = documentRepository;
        _documentShareRepository = documentShareRepository;
        _auditService = auditService;
        _logger = logger;
    }

    public async Task<Unit> Handle(UnshareDocumentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Removing share {ShareId} from document {DocumentId}", request.ShareId, request.DocumentId);

        var document = await _documentRepository.GetByIdAsync(request.DocumentId, cancellationToken);
        if (document is null)
        {
            _logger.LogWarning("Document {DocumentId} not found for unshare", request.DocumentId);
            throw new NotFoundException("Document not found.");
        }

        if (document.OwnerSub != request.CurrentUserSub && !string.Equals(request.CurrentUserRole, "Admin", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("User {User} not allowed to unshare document {DocumentId}", request.CurrentUserEmail, request.DocumentId);
            throw new AuthorizationException("You are not allowed to unshare this document.");
        }


        var documentShared = await _documentShareRepository.GetByIdAsync(request.ShareId, cancellationToken);

        await _documentShareRepository.RemoveAsync(documentShared!, cancellationToken);

        _logger.LogInformation("Share {ShareId} removed from document {DocumentId}", request.ShareId, request.DocumentId);

        await _auditService.LogAsync(request.CurrentUserSub, request.CurrentUserEmail,
            "DocumentUnshared", nameof(DocumentShare), request.DocumentId.ToString(),
            new { request.ShareId });

        return Unit.Value;
    }
}