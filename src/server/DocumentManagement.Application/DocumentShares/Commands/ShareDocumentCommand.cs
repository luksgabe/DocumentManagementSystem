using DocumentManagement.Application.DocumentShares.DTOs;
using DocumentManagement.Core.Enuns;
using MediatR;

namespace DocumentManagement.Application.DocumentShares.Commands;

public record ShareDocumentCommand(
    Guid DocumentId,
    TargetType TargetType,
    string TargetValue,
    Permission Permission,
    string CurrentUserSub,
    string CurrentUserEmail,
    string CurrentUserRole
) : IRequest<DocumentShareDto>;