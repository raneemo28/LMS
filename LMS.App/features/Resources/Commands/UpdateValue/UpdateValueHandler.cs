using AutoMapper;
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.Resources.Commands.UpdateValue;

public class UpdateValueHandler : IRequestHandler<UpdateValueCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateValueHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateValueCommand request, CancellationToken cancellationToken)
    {
        var value = await _unitOfWork.Resources.GetValueByIdAsync(request.ValueId, request.ResourceId);
        if (value == null) return false;

        var property = await _unitOfWork.Vocabularies.GetPropertyByIdAsync(value.PropertyId);
        var systemUri = property?.TermUri;

        _mapper.Map(request.Dto, value);

        value.ValueUri = systemUri;

        _unitOfWork.Resources.UpdateValue(value);
        
        return await _unitOfWork.CommitAsync() > 0;
    }
}