using MediatR;

namespace DocumentManagement.Application.Documents.Commands;

public sealed record DeleteDocumentCommand(Guid Id) : IRequest<Unit>;