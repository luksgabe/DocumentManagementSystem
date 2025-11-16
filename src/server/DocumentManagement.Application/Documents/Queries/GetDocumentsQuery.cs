using DocumentManagement.Application.Documents.DTOs;
using MediatR;

namespace DocumentManagement.Application.Documents.Queries;

public sealed record GetDocumentsQuery(
    string? Search,
    string? Tag,
    string? ContentType,
    int Page = 1,
    int PageSize = 20
) : IRequest<DocumentListResultDto>;
