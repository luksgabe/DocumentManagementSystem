using DocumentManagement.Application.DocumentShares.DTOs;
using DocumentManagement.Core.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using DocumentManagement.Core.Entities;
using DocumentManagement.Core.Exceptions;
using DocumentManagement.Core.Interfaces.Services;

namespace DocumentManagement.Application.DocumentShares.Commands;

public class ShareDocumentCommandHandler
    : IRequestHandler<ShareDocumentCommand, DocumentShareDto>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IAuditService _auditService;
    private readonly ILogger<ShareDocumentCommandHandler> _logger;

    public ShareDocumentCommandHandler(
        IDocumentRepository documentRepository,
        IAuditService auditService,
        ILogger<ShareDocumentCommandHandler> logger)
    {
        _documentRepository = documentRepository;
        _auditService = auditService;
        _logger = logger;
    }

    public async Task<DocumentShareDto> Handle(ShareDocumentCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sharing document {DocumentId} with {TargetType}={TargetValue}",
            request.DocumentId, request.TargetType, request.TargetValue);

        var document = await _documentRepository.GetByIdAsync(request.DocumentId, cancellationToken);
        if (document is null)
        {
            _logger.LogWarning("Document {DocumentId} not found", request.DocumentId);
            throw new NotFoundException("Document not found.");
        }

        if (document.OwnerSub != request.CurrentUserSub && !string.Equals(request.CurrentUserRole, "Admin", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("User {User} not allowed to share document {DocumentId}", request.CurrentUserEmail, request.DocumentId);
            throw new AuthorizationException("You are not allowed to share this document.");
        }

        var share = new DocumentShare(
            Guid.NewGuid(),
            document.Id,
            request.TargetType,
            request.TargetValue,
            request.Permission);

        document.AddShare(share); 

        await _documentRepository.UpdateDocumentAsync(document, document.DocumentTags.Select(dt => dt.Tag.Name), cancellationToken);

        _logger.LogInformation("Document {DocumentId} shared successfully with {TargetType}={TargetValue}",
            document.Id, request.TargetType, request.TargetValue);

        await _auditService.LogAsync(request.CurrentUserSub, request.CurrentUserEmail,
            "DocumentShared", nameof(DocumentShare), request.DocumentId.ToString(),
            new { request.TargetType, request.TargetValue, request.Permission });

        return new DocumentShareDto(
            share.Id,
            share.TargetType,
            share.TargetValue,
            share.Permission,
            share.SharedAt
        );
    }
}
