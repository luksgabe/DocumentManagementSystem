using DocumentManagement.Application.Documents.DTOs;
using MediatR;

namespace DocumentManagement.Application.Documents.Queries;
public sealed record DownloadDocumentQuery(Guid Id) : IRequest<DocumentDownloadDto>;