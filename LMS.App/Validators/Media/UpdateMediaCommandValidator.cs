using FluentValidation;
using LMS.App.Features.Medias.Commands.UpdateMediaCommand;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Media;

public class UpdateMediaCommandValidator : AbstractValidator<UpdateMediaCommand>
{
    public UpdateMediaCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Dto.Id).GreaterThan(0).WithMessage(localizer["ValidMediaIdRequired"]);
        RuleFor(x => x.Dto.FileName)
            .NotEmpty().WithMessage(localizer["FileNameEmpty"])
            .MaximumLength(255).WithMessage(localizer["FileNameMaxLength"]);

        RuleForEach(x => x.Dto.Values).ChildRules(value =>
        {
            value.RuleFor(v => v.PropertyId).GreaterThan(0);
            value.RuleFor(v => v.Type).NotEmpty()
                .MaximumLength(10).WithMessage(localizer["TypeMaxLength"]);
        });
    }
}