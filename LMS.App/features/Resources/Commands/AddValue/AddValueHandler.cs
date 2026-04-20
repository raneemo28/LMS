using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.Resources.Commands.AddValue;

public class AddValueHandler : IRequestHandler<AddValueCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddValueHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(AddValueCommand request, CancellationToken cancellationToken)
    {
        var value = await _unitOfWork.Resources.AddValueAsync(
            request.ResourceId,
            request.PropertyId,
            request.ValueText,
            request.ValueUri,
            request.ValueResourceId,
            request.Type,
            request.Language
        );
        if(value == null) return false;
        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}
