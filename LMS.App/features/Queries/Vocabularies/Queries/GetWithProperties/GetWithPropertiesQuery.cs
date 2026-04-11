using LMS.Domain.Entities;
using MediatR;

namespace LMS.Application.Features.Vocabularies.Queries.GetWithProperties;

// نطلب المعرف الخاص بالقاموس لجلب بياناته مع خصائصه
public record GetWithPropertiesQuery(int Id) : IRequest<Vocabulary>;