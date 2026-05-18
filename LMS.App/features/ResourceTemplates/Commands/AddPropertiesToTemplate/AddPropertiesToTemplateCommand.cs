using MediatR;
using System.Collections.Generic;
using LMS.App.DTOs.ResourceProperty;

namespace LMS.App.Features.ResourceTemplates.Commands.AddPropertiesToTemplate;

public record AddPropertiesToTemplateCommand(
    int TemplateId,
    List<PropertyToTemplateInput> Properties
) : IRequest<bool>;
