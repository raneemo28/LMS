using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.Resources.Commands.RemoveValue;

public class RemoveValueHandler : IRequestHandler<RemoveValueCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveValueHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(RemoveValueCommand request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.Resources.RemoveValueAsync(
            request.ResourceId,
            request.ValueId
        );

        if (result)
            await _unitOfWork.CommitAsync();

        return result;
    }
}
