using DocumentManagement.Core.Enuns;

namespace DocumentManagement.Application.DocumentShares.DTOs;

public record ShareDocumentRequestDto(
    TargetType TargetType,
    string TargetValue,
    Permission Permission
);