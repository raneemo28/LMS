using MediatR;
using LMS.App.DTOs.Vocabulary;
using System.Collections.Generic;

namespace LMS.App.Features.Vocabularies.Queries.GetVocabularyByPrefix;
public record GetVocabularyByPrefixQuery(string Prefix) : IRequest<List<VocabularyDto>>;
