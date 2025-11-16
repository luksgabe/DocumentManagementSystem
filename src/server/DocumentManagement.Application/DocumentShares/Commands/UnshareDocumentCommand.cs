using MediatR;

namespace DocumentManagement.Application.DocumentShares.Commands;

public sealed record UnshareDocumentCommand(
    Guid DocumentId,
    Guid ShareId,
    string CurrentUserSub,
    string CurrentUserEmail,
    string CurrentUserRole
) : IRequest<Unit>;