namespace DocumentManagement.Application.Documents.DTOs;

public record DocumentDto(Guid Id,
    string Title,
    string? Description,
    AccessType AccessType,
    string OwnerEmail,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc,
    string[] Tags,
    long FileSizeBytes,
    string ContentType
    );