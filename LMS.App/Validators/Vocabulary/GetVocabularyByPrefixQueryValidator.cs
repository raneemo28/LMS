using FluentValidation;
using LMS.App.Features.Vocabularies.Queries.GetVocabularyByPrefix;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Vocabulary;

public class GetVocabularyByPrefixQueryValidator : AbstractValidator<GetVocabularyByPrefixQuery>
{
    public GetVocabularyByPrefixQueryValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Prefix).NotEmpty().WithMessage(localizer["PrefixRequiredForSearch"]).Matches(@"^[a-z]+$").WithMessage(localizer["PrefixLowercaseAlpha"]);
    }
}