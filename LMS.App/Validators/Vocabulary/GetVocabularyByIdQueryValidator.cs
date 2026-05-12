using FluentValidation;
using LMS.App.Features.Vocabularies.Queries.GetVocabularyById;

namespace LMS.App.Validators.Vocabularies;

public class GetVocabularyByIdQueryValidator : AbstractValidator<GetVocabularyByIdQuery>
{
    public GetVocabularyByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Vocabulary ID is required.")
            .GreaterThan(0).WithMessage("Vocabulary ID must be a valid positive integer.");
    }
}
