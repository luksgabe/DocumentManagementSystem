using DocumentManagement.Application.DocumentShares.Commands;
using FluentValidation;

namespace DocumentManagement.Application.DocumentShares.Validations;

public class UnshareDocumentCommandValidator : AbstractValidator<UnshareDocumentCommand>
{
    public UnshareDocumentCommandValidator()
    {
        RuleFor(x => x.DocumentId).NotEmpty();
        RuleFor(x => x.ShareId).NotEmpty();
    }
}