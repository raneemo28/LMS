using FluentValidation;
using LMS.App.features.Medias.Queries.GetMediaByOwner;

namespace LMS.App.Validators.Media;

public class GetMediaByOwnerQueryValidator : AbstractValidator<GetMediaByOwnerQuery>
{
    public GetMediaByOwnerQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required to fetch media.")
            .MinimumLength(5).WithMessage("Invalid User ID format.");
    }
}
