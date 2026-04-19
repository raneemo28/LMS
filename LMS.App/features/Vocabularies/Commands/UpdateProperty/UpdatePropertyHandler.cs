using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.Application.Features.Vocabularies.Commands.UpdateProperty;

public class UpdatePropertyHandler : IRequestHandler<UpdatePropertyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    public UpdatePropertyHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> Handle(UpdatePropertyCommand request, CancellationToken ct)
   {
    var prop = await _unitOfWork.Vocabularies.GetPropertyByIdAsync(request.Id);
    if (prop == null) return false;

    prop.LocalName = request.LocalName;
    prop.Label = request.Label;
    prop.TermUri = request.TermUri;
    
    _unitOfWork.Vocabularies.UpdateProperty(prop);
    return await _unitOfWork.CommitAsync() > 0;
    }
}