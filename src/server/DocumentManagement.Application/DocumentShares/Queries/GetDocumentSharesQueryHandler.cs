using DocumentManagement.Application.DocumentShares.DTOs;
using DocumentManagement.Core.Exceptions;
using DocumentManagement.Core.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.Application.DocumentShares.Queries;

public class GetDocumentSharesQueryHandler
    : IRequestHandler<GetDocumentSharesQuery, IReadOnlyList<DocumentShareDto>>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IDocumentShareRepository _documentShareRepository;
    private readonly ILogger<GetDocumentSharesQueryHandler> _logger;

    public GetDocumentSharesQueryHandler(
        IDocumentRepository documentRepository,
        IDocumentShareRepository documentShareRepository,
        ILogger<GetDocumentSharesQueryHandler> logger)
    {
        _documentRepository = documentRepository;
        _documentShareRepository = documentShareRepository;
        _logger = logger;
    }

    public async Task<IReadOnlyList<DocumentShareDto>> Handle(GetDocumentSharesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listing shares for document {DocumentId}", request.DocumentId);

        var document = await _documentRepository.GetByIdAsync(request.DocumentId, cancellationToken);

        if (document is null)
        {
            throw new NotFoundException("Document not found.");
        }

        var documentShares = await _documentShareRepository.GetByDocumentId(request.DocumentId, cancellationToken);
        if (documentShares is null)
        {
            _logger.LogWarning("Document {DocumentId} not found when listing shares", request.DocumentId);
            throw new NotFoundException("Document share not found.");
        }

        return documentShares
            .Select(s => new DocumentShareDto(
                s.Id,
                s.TargetType,
                s.TargetValue,
                s.Permission,
                s.SharedAt))
            .ToList();
    }
}
