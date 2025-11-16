using DocumentManagement.Core.Enuns;

namespace DocumentManagement.Application.DocumentShares.DTOs;

public record DocumentShareDto(
    Guid Id,
    TargetType TargetType,
    string TargetValue,
    Permission Permission,
    DateTime SharedAt
);