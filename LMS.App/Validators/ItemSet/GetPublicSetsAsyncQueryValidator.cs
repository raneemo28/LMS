using FluentValidation;
using LMS.App.Features.ItemSets.Queries.GetPublicSetsAsync;

namespace LMS.App.Validators.ItemSets;
public class GetPublicSetsAsyncQueryValidator : AbstractValidator<GetPublicSetsAsyncQuery>
{
    public GetPublicSetsAsyncQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required to fetch relevant public sets.")
            .NotEmpty().WithMessage("UserId is required to fetch relevant public sets.");
    }
}
