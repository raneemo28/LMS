using FluentValidation;
using LMS.App.Features.Resources.Queries.GetResourceValues;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Resources;

public class GetResourceValuesQueryValidator : AbstractValidator<GetResourceValuesQuery>
{
    public GetResourceValuesQueryValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.ResourceId).NotEmpty().WithMessage(localizer["ResourceIdRequired"])
            .GreaterThan(0).WithMessage(localizer["ResourceIdPositiveInteger"]);
    }
}