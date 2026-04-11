using LMS.Domain.Entities;
using MediatR;

namespace LMS.Application.Features.Vocabularies.Queries.GetWithProperties;

public record GetWithPropertiesQuery(int Id) : IRequest<Vocabulary>;