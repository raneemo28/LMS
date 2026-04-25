using FluentValidation;
using LMS.Application.Features.Vocabularies.Queries.GetVocabularyByPrefix;

namespace LMS.Application.Validators.Vocabularie;

public class GetVocabularyByPrefixQueryValidator : AbstractValidator<GetVocabularyByPrefixQuery>
{
    public GetVocabularyByPrefixQueryValidator()
    {
        RuleFor(x => x.Prefix)
            .NotEmpty().WithMessage("Prefix is required for searching.")
            .Matches(@"^[a-z]+$").WithMessage("Prefix must be lowercase alphabetic characters only.");
    }
}