using LMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Vocabularies.Commands.DeleteProperty;

public class DeletePropertyHandler : IRequestHandler<DeletePropertyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public DeletePropertyHandler(IUnitOfWork unitOfWork, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<bool> Handle(DeletePropertyCommand request, CancellationToken ct)
    {
        if (await _unitOfWork.Vocabularies.HasLinkedValuesAsync(request.Id))
            throw new InvalidOperationException(_localizer["CannotDeleteLinkedProperty"]);

        var deleted = await _unitOfWork.Vocabularies.DeletePropertyAsync(request.Id);
        return deleted && await _unitOfWork.CommitAsync() > 0;
    }
}