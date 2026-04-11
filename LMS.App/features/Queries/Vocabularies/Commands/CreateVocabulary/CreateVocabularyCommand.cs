using MediatR;

namespace LMS.Application.Features.Vocabularies.Commands.CreateVocabulary;

public record CreateVocabularyCommand(
    string Prefix, 
    string NamespaceUri, 
    string Label
) : IRequest<int>; // سيعيد المعرف (Id) الخاص بالعنصر الجديد