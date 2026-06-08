using AutoMapper;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.Vocabularies.Commands.UpdateProperty;

public class UpdatePropertyHandler : IRequestHandler<UpdatePropertyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePropertyHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdatePropertyCommand request, CancellationToken ct)
    {
        var prop = await _unitOfWork.Vocabularies.GetPropertyByIdAsync(request.Id);
        if (prop == null) return false;

        _mapper.Map(request.Dto, prop);

        _unitOfWork.Vocabularies.UpdateProperty(prop);
        return await _unitOfWork.CommitAsync() > 0;
    }
}