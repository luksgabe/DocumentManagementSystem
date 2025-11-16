using DocumentManagement.Application.Documents.DTOs;
using MediatR;

namespace DocumentManagement.Application.Documents.Commands;

public record UpdateDocumentCommand(
    Guid Id,
    string Title,
    string? Description,
    AccessType AccessType,
    string[] Tags
) : IRequest<DocumentDto>;