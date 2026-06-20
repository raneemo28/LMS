using FluentValidation;
using LMS.App.Features.Users.Queries;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Validators.Users;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator(IStringLocalizer<ErrorMessages> localizer)
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(localizer["UserIdRequired"]);
    }
}
