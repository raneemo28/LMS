using MediatR;
using LMS.Domain.Interfaces;

namespace LMS.Application.Features.Vocabularies.Commands.UpdateProperty;

public class UpdatePropertyHandler : IRequestHandler<UpdatePropertyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePropertyHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
    {
        var updatedProperty = await _unitOfWork.Vocabularies.UpdatePropertyAync(
            request.PropertyId, 
            request.LocalName, 
            request.Label, 
            request.TermUri
        );

        if (updatedProperty == null)
        {
            return false;
        }

        var result = await _unitOfWork.CommitAsync();

        return result > 0;
    }
}