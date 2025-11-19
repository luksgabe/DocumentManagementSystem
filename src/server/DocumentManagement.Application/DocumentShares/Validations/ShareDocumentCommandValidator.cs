using DocumentManagement.Application.DocumentShares.Commands;
using DocumentManagement.Core.Enuns;
using FluentValidation;

namespace DocumentManagement.Application.DocumentShares.Validations;

public class ShareDocumentCommandValidator : AbstractValidator<ShareDocumentCommand>
{
    public ShareDocumentCommandValidator()
    {
        RuleFor(x => x.DocumentId)
            .NotEmpty();

        RuleFor(x => x.TargetType)
            .IsInEnum();

        RuleFor(x => x.TargetValue)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Permission)
            .IsInEnum()
            .Must(p => p is Permission.Read or Permission.Write or Permission.Share or Permission.Delete)
            .WithMessage("Only Read, Write, Share or Delete permissions are allowed for this operation.");
    }
}