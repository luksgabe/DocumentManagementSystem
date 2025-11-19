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
    private readonly IDocumentShareRepository _documentShareRepository;
    private readonly IAuditService _auditService;
    private readonly ILogger<ShareDocumentCommandHandler> _logger;

    public ShareDocumentCommandHandler(
        IDocumentRepository documentRepository,
        IDocumentShareRepository documentShareRepository,
        IAuditService auditService,
        ILogger<ShareDocumentCommandHandler> logger)
    {
        _documentRepository = documentRepository;
        _documentShareRepository = documentShareRepository;
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

        var sharedDocuments = await _documentShareRepository
            .GetCustomData(p => p.DocumentId == request.DocumentId
                && p.TargetValue == request.TargetValue
                &&  p.TargetType == request.TargetType
                && p.Permission == request.Permission, cancellationToken);

        if(sharedDocuments.Any())
        {
            _logger.LogWarning("Document {DocumentId} is already shared with {TargetType}={TargetValue}",
                request.DocumentId, request.TargetType, request.TargetValue);
            throw new AppValidationException("Document is already shared with the specified target and permission.");
        }


        var newShare = new DocumentShare(
            Guid.NewGuid(),
            document.Id,
            request.TargetType,
            request.TargetValue,
            request.Permission);

        await _documentShareRepository.AddAsync(newShare, cancellationToken);

        await _documentRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Document {DocumentId} shared successfully with {TargetType}={TargetValue}",
            document.Id, request.TargetType, request.TargetValue);

        await _auditService.LogAsync(request.CurrentUserSub, request.CurrentUserEmail,
            "DocumentShared", nameof(DocumentShare), request.DocumentId.ToString(),
            new { request.TargetType, request.TargetValue, request.Permission });

        return new DocumentShareDto(
            newShare.Id,
            newShare.TargetType,
            newShare.TargetValue,
            newShare.Permission,
            newShare.SharedAt
        );
    }
}
