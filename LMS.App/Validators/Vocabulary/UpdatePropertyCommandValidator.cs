using FluentValidation;
using LMS.App.Features.Vocabularies.Commands.UpdateProperty;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Vocabulary;

public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
{
    public UpdatePropertyCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage(localizer["PropertyIdValid"]);
        RuleFor(x => x.Dto).NotNull().WithMessage(localizer["PropertyDataRequired"]);
        RuleFor(x => x.Dto.LocalName).NotEmpty().WithMessage(localizer["LocalNameRequired"]).Matches(@"^[a-zA-Z0-9_]+$").WithMessage(localizer["LocalNameAlphanumeric"]);
        RuleFor(x => x.Dto.Label).NotEmpty().WithMessage(localizer["LabelRequired"]).MaximumLength(100);
        RuleFor(x => x.Dto.TermUri).NotEmpty().WithMessage(localizer["TermUriRequired"]).Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage(localizer["TermUriAbsolute"]);
    }
}