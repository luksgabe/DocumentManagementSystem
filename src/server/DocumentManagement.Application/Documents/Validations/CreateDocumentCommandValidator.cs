using DocumentManagement.Application.Documents.Commands;
using FluentValidation;

namespace DocumentManagement.Application.Documents.Validations;

public class CreateDocumentCommandValidator : AbstractValidator<CreateDocumentCommand>
{
    public CreateDocumentCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.AccessType)
            .IsInEnum();

        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required.")
            .Must(f => f.Length <= 10 * 1024 * 1024)
            .WithMessage("File size must be <= 10MB.")
            .Must(f =>
            {
                var ext = Path.GetExtension(f.FileName).ToLowerInvariant();
                return new[] { ".pdf", ".docx", ".txt" }.Contains(ext);
            })
            .WithMessage("Only .pdf, .docx and .txt files are allowed.");

        RuleForEach(x => x.Tags)
            .MaximumLength(80);
    }
}