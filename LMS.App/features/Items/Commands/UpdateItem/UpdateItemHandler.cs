using MediatR;
using LMS.Domain.Interfaces;
using LMS.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Items.Commands.UpdateItem;

public class UpdateItemHandler : IRequestHandler<UpdateItemCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public UpdateItemHandler(IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<bool> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Items.GetItemWithFullDataForUpdateAsync(request.Dto.Id);
        if (item == null) return false;

        if (item.OwnerId != request.OwnerId)
            throw new UnauthorizedAccessException(_localizer["NotAuthorizedUpdateItem"]);

        _mapper.Map(request.Dto, item);
        item.ModifiedAt = DateTime.UtcNow;
        item.ModifiedBy = request.OwnerId;
        _unitOfWork.Items.Update(item);
        return await _unitOfWork.CommitAsync() > 0;
    }
}