using FluentValidation;
using LMS.App.Features.Vocabularies.Commands.UpdateVocabulary;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Vocabulary;

public class UpdateVocabularyCommandValidator : AbstractValidator<UpdateVocabularyCommand>
{
    public UpdateVocabularyCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localizer["ValidVocabularyIdRequired"]);
        RuleFor(x => x.Dto.Prefix).NotEmpty().WithMessage(localizer["PrefixRequired"]).MaximumLength(20).WithMessage(localizer["PrefixMaxLength"]);
        RuleFor(x => x.Dto.NamespaceUri).NotEmpty().WithMessage(localizer["NamespaceUriRequired"]).Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage(localizer["NamespaceUriAbsolute"]);
        RuleFor(x => x.Dto.Label).NotEmpty().WithMessage(localizer["LabelRequired"]).MaximumLength(100).WithMessage(localizer["LabelCannotExceed100"]);
    }
}