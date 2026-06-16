using FluentValidation;
using LMS.App.Features.Media.Commands.CreateMediaCommand;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Media;

public class CreateMediaCommandValidator : AbstractValidator<CreateMediaCommand>
{
    public CreateMediaCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.OwnerId).NotEmpty().WithMessage(localizer["OwnerIdRequired"]);
        RuleFor(x => x.Dto).NotNull().WithMessage(localizer["MediaDataRequired"]);
        RuleFor(x => x.Dto.FileName)
            .NotEmpty().WithMessage(localizer["FileNameRequired"])
            .MaximumLength(255).WithMessage(localizer["FileNameMaxLength"]);
    }
}