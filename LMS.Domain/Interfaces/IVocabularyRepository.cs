using LMS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Domain.Interfaces;

public interface IVocabularyRepository : IGenericRepository<Vocabulary>
{
    // Property Management
    Task<Property?> GetPropertyByIdAsync(int id);
    Task AddPropertyAsync(Property property);
    void UpdateProperty(Property property);
    Task<bool> DeletePropertyAsync(int propertyId);
    
    // Queries
    Task<IEnumerable<Property>> GetPropertiesByVocabularyIdAsync(int vocabularyId);
    Task<IEnumerable<Property>> GetAllPropertiesAsync();
    
    // Validations
    Task<bool> HasLinkedValuesAsync(int propertyId);
    Task<bool> IsPropertyExistsInVocabularyAsync(int vocabularyId, string localName);
    Task<bool> IsLabelUniqueAsync(string label);
    Task<bool> IsNamespaceUriUniqueAsync(string uri);
}