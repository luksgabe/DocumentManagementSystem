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
    private readonly IAuditService _auditService;
    private readonly ILogger<UnshareDocumentCommandHandler> _logger;

    public UnshareDocumentCommandHandler(
        IDocumentRepository documentRepository,
        IAuditService auditService,
        ILogger<UnshareDocumentCommandHandler> logger)
    {
        _documentRepository = documentRepository;
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

        document.RemoveShare(request.ShareId);

        await _documentRepository.UpdateDocumentAsync(document, document.DocumentTags.Select(dt => dt.Tag.Name), cancellationToken);

        _logger.LogInformation("Share {ShareId} removed from document {DocumentId}", request.ShareId, request.DocumentId);

        await _auditService.LogAsync(request.CurrentUserSub, request.CurrentUserEmail,
            "DocumentUnshared", nameof(DocumentShare), request.DocumentId.ToString(),
            new { request.ShareId });

        return Unit.Value;
    }
}