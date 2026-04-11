using LMS.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace LMS.Application.Features.Vocabularies.Queries.GetByPrefix;

public record GetByPrefixQuery(string Prefix) : IRequest<List<Vocabulary>>;