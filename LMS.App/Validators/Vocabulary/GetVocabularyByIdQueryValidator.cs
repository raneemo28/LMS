using FluentValidation;
using LMS.App.Features.Vocabularies.Queries.GetVocabularyById;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Vocabularies;

public class GetVocabularyByIdQueryValidator : AbstractValidator<GetVocabularyByIdQuery>
{
    public GetVocabularyByIdQueryValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(localizer["VocabularyIdRequired"]).GreaterThan(0).WithMessage(localizer["VocabularyIdPositiveInteger"]);
    }
}