using FluentValidation;
using LMS.App.Features.Medias.Commands.UpdateMediaCommand;

namespace LMS.App.Validators.Media;

public class UpdateMediaCommandValidator : AbstractValidator<UpdateMediaCommand>
{
    public UpdateMediaCommandValidator()
    {
        RuleFor(x => x.Dto.Id)
            .GreaterThan(0).WithMessage("Valid Media ID is required.");

        RuleFor(x => x.Dto.FileName)
            .NotEmpty().WithMessage("FileName cannot be empty.")
            .MaximumLength(255).WithMessage("FileName cannot exceed 255 characters.");

        /*RuleFor(x => x.Dto.CurrentUserId)
            .NotEmpty().WithMessage("Current user identity is required.");*/

        RuleForEach(x => x.Dto.Values).ChildRules(value =>
        {
            value.RuleFor(v => v.PropertyId).GreaterThan(0);
            value.RuleFor(v => v.Type).NotEmpty()
            .MaximumLength(10).WithMessage("Type cannot exceed 10 characters.");
        });
    }
}
