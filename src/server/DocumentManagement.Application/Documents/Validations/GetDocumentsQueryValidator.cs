using DocumentManagement.Application.Documents.Queries;
using FluentValidation;

namespace DocumentManagement.Application.Documents.Validations;

public sealed class GetDocumentsQueryValidator : AbstractValidator<GetDocumentsQuery>
{
    public GetDocumentsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}