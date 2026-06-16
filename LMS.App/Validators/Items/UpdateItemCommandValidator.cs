using FluentValidation;
using LMS.App.Features.Items.Commands.UpdateItem;
using LMS.Domain.Constants;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Items;

public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.OwnerId).NotEmpty().WithMessage(localizer["OwnerIdRequired"]);
        RuleFor(x => x.Dto).NotNull().WithMessage(localizer["ItemDataRequired"]);
        RuleFor(x => x.Dto.Id).GreaterThan(0).WithMessage(localizer["ItemIdGreaterThanZero"]);
        RuleFor(x => x.Dto.TemplateId).GreaterThan(0).WithMessage(localizer["TemplateIdGreaterThanZero"]);
        RuleFor(x => x.Dto.Values)
            .NotNull().WithMessage(localizer["ValuesListNotNull"])
            .Must(v => v != null && v.Any()).WithMessage(localizer["ValueRequired"]);

        RuleForEach(x => x.Dto.Values).ChildRules(value =>
        {
            value.RuleFor(v => v.PropertyId).GreaterThan(0).WithMessage(localizer["PropertyIdValid"]);
            value.RuleFor(v => v.Type)
                .NotEmpty().WithMessage(localizer["TypeRequired"])
                .Must(t => new[] { SystemConstants.TypeText, SystemConstants.TypeUri, SystemConstants.TypeResource }.Contains(t))
                .WithMessage(localizer["TypeMustBeTextUriResource"]);
        });
    }
}