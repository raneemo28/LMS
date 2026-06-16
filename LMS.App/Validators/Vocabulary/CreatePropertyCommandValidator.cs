using FluentValidation;
using LMS.App.Features.Vocabularies.Commands.CreateProperty;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Vocabulary;

public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
{
    public CreatePropertyCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.VocabularyId).GreaterThan(0).WithMessage(localizer["VocabularyIdValid"]);
        RuleFor(x => x.Dto).NotNull().WithMessage(localizer["PropertyDataRequired"]);
        RuleFor(x => x.Dto.LocalName).NotEmpty().WithMessage(localizer["LocalNameRequired"]).Matches(@"^[a-zA-Z0-9_]+$").WithMessage(localizer["LocalNameAlphanumeric"]);
        RuleFor(x => x.Dto.Label).NotEmpty().WithMessage(localizer["LabelRequired"]).MaximumLength(255).WithMessage(localizer["LabelMaxLength"]);
        RuleFor(x => x.Dto.TermUri).NotEmpty().WithMessage(localizer["TermUriRequired"]).Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage(localizer["TermUriAbsolute"]);
    }
}