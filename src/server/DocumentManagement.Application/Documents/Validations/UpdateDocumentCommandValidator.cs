using DocumentManagement.Application.Documents.Commands;
using FluentValidation;

namespace DocumentManagement.Application.Documents.Validations;

public class UpdateDocumentCommandValidator : AbstractValidator<UpdateDocumentCommand>
{
    public UpdateDocumentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.AccessType)
            .IsInEnum();

        RuleForEach(x => x.Tags)
            .MaximumLength(80);
    }
}