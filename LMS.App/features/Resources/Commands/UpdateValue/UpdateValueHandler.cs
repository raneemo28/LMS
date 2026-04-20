using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.Resources.Commands.UpdateValue;

public class UpdateValueHandler : IRequestHandler<UpdateValueCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateValueHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateValueCommand request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.Resources.UpdateValueAsync(
            request.ResourceId,
            request.ValueId,
            request.ValueText,
            request.ValueUri,
            request.ValueResourceId,
            request.Type,
            request.Language
        );

        if (result)
            await _unitOfWork.CommitAsync();

        return result;
    }
}
