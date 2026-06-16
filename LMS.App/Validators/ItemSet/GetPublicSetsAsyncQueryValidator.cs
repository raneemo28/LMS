using FluentValidation;
using LMS.App.Features.ItemSets.Queries.GetPublicSetsAsync;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.ItemSets;

public class GetPublicSetsAsyncQueryValidator : AbstractValidator<GetPublicSetsAsyncQuery>
{
    public GetPublicSetsAsyncQueryValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage(localizer["UserIdRequiredPublicSets"]);
    }
}