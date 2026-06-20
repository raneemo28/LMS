using AutoMapper;
using LMS.Domain.Entities;  
using LMS.Domain.Interfaces;
using MediatR;

namespace LMS.App.Features.Media.Commands.CreateMediaCommand;

public class CreateMediaHandler : IRequestHandler<CreateMediaCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateMediaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateMediaCommand request, CancellationToken cancellationToken)
    {
        var media = _mapper.Map<LMS.Domain.Entities.Media>(request.Dto);
        media.Type = "Media";
        media.OwnerId = request.OwnerId;
        media.CreatedAt = DateTime.UtcNow;
        media.CreatedBy = request.OwnerId;

        await _unitOfWork.Media.AddAsync(media);
        await _unitOfWork.CommitAsync();

        return media.Id;
    }
}