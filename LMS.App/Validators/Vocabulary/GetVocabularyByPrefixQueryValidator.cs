using FluentValidation;
using LMS.App.Features.Vocabularies.Queries.GetVocabularyByPrefix;

namespace LMS.App.Validators.Vocabulary;

public class GetVocabularyByPrefixQueryValidator : AbstractValidator<GetVocabularyByPrefixQuery>
{
    public GetVocabularyByPrefixQueryValidator()
    {
        RuleFor(x => x.Prefix)
            .NotEmpty().WithMessage("Prefix is required for searching.")
            .Matches(@"^[a-z]+$").WithMessage("Prefix must be lowercase alphabetic characters only.");
    }
}
