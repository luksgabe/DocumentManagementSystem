using DocumentManagement.Application.DocumentShares.DTOs;
using MediatR;

namespace DocumentManagement.Application.DocumentShares.Queries;

public record GetDocumentSharesQuery(Guid DocumentId)
    : IRequest<IReadOnlyList<DocumentShareDto>>;
