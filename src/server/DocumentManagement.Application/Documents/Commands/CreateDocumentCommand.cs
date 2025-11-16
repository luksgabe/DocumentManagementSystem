using DocumentManagement.Application.Documents.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DocumentManagement.Application.Documents.Commands;

public record CreateDocumentCommand(
    string Title,
    string? Description,
    AccessType AccessType,
    IFormFile File,
    string[] Tags,
    string OwnerSub,
    string OwnerEmail
) : IRequest<DocumentDto>;