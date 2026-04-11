using LMS.Domain.Entities;
namespace LMS.Domain.Interfaces
{
    public interface IVocabularyRepository : IGenericRepository<Vocabulary>
    {
        Task <Property> AddPropertyAync(int vocabularyId, string localName, string label, string TermUri);
        Task<bool> IsPropertyExistsInVocabularyAsync(int vocabularyId, string localName);
        Task<Vocabulary?> GetByPrefixAsync(string prefix);
        Task<bool> IsLabelUniqueAsync(string label);
        Task<bool> IsNamespaceUriUniqueAsync(string uri);
        Task<Object?> GetWithPropertiesAsync(int id);
        
    }
}