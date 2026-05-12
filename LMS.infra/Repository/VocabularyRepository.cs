using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.Infra.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Infra.Repository;

public class VocabularyRepository : GenericRepository<Vocabulary>, IVocabularyRepository
{
    public VocabularyRepository(LibraryDbContext context) : base(context) { }

    // --- Property Commands ---
    public async Task AddPropertyAsync(Property property)
    {
        bool existsLocal = await IsPropertyExistsInVocabularyAsync(property.VocabularyId, property.LocalName);
        if (existsLocal)
            throw new InvalidOperationException($"Property '{property.LocalName}' already exists in this vocabulary.");

        await _context.Properties.AddAsync(property);
    }
    public async Task<Property?> GetPropertyByIdAsync(int id) {
        return await _context.Properties.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    }
    public void UpdateProperty(Property property) => _context.Properties.Update(property);

    public async Task<bool> DeletePropertyAsync(int propertyId)
    {
        var property = await _context.Properties.FindAsync(propertyId);
        if (property == null) return false;
        _context.Properties.Remove(property);
        return true;
    }

    // --- Queries ---
    public async Task<IEnumerable<Property>> GetPropertiesByVocabularyIdAsync(int vocabularyId) =>
        await _context.Properties
            .Where(p => p.VocabularyId == vocabularyId)
            .AsNoTracking()
            .ToListAsync();

    // --- Validations ---
    public async Task<bool> HasLinkedValuesAsync(int propertyId) =>
        await _context.Values.AnyAsync(v => v.PropertyId == propertyId);

    public async Task<bool> IsPropertyExistsInVocabularyAsync(int vocabularyId, string localName) =>
        await _context.Properties.AsNoTracking()
            .AnyAsync(p => p.VocabularyId == vocabularyId && p.LocalName == localName);

    public async Task<bool> IsLabelUniqueAsync(string label) =>
        await _context.Vocabularies.AsNoTracking().AllAsync(v => v.Label != label);

    public async Task<bool> IsNamespaceUriUniqueAsync(string uri) =>
        await _context.Vocabularies.AsNoTracking().AllAsync(v => v.NamespaceUri != uri);
}
