using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using MediatR;
using LMS.App.DTOs.Value;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LMS.App.Features.Resources.Commands.AddValues;

public class AddValuesHandler : IRequestHandler<AddValuesCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddValuesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(AddValuesCommand request, CancellationToken cancellationToken)
    {
        foreach (var val in request.Values)
        {
            var property = await _unitOfWork.Vocabularies.GetPropertyByIdAsync(val.PropertyId);
            if (property == null)
                throw new KeyNotFoundException($"Property {val.PropertyId} not found.");

            // System constructs the URI from the property definition — not user-supplied.
            var systemUri = property.TermUri;

            var value = await _unitOfWork.Resources.AddValueAsync(
                request.ResourceId,
                val.PropertyId,
                val.ValueText,
                systemUri,
                val.ValueResourceId,
                val.Type,
                val.Language
            );
            if (value == null) return false;
        }

        return await _unitOfWork.CommitAsync() > 0;
    }
}
