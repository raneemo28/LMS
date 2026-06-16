using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;
using LMS.App.DTOs.Value;
using Microsoft.Extensions.Localization;
using LMS.App.shared_resources;

namespace LMS.App.Features.Resources.Commands.AddValues;

public class AddValuesHandler : IRequestHandler<AddValuesCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public AddValuesHandler(IUnitOfWork unitOfWork, IStringLocalizer<ErrorMessages> localizer)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<bool> Handle(AddValuesCommand request, CancellationToken cancellationToken)
    {
        foreach (var val in request.Values)
        {
            var property = await _unitOfWork.Vocabularies.GetPropertyByIdAsync(val.PropertyId);
            if (property == null)
                throw new KeyNotFoundException(_localizer["ResourceNotFound"]);

            var systemUri = property.TermUri;
            var value = await _unitOfWork.Resources.AddValueAsync(
                request.ResourceId, val.PropertyId, val.ValueText, systemUri, val.ValueResourceId, val.Type, val.Language);
            
            if (value == null) return false;
        }
        return await _unitOfWork.CommitAsync() > 0;
    }
}