using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.infra.Database;
using Microsoft.EntityFrameworkCore;

namespace LMS.infra.Repository
{
    public class VocabularyRepository : GenericRepository<Vocabulary>, IVocabularyRepository
    {
        public VocabularyRepository(LibraryDbContext context) : base(context) { }

        public async Task<Vocabulary?> GetByPrefixAsync(string prefix)
        {
            return await _context.Vocabularies
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Prefix == prefix);
        }

        public async Task<object?> GetWithPropertiesAsync(int id)
        {
            var vocabulary = await _context.Vocabularies
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vocabulary == null) return null;

            var properties = await _context.Properties
                .Where(p => p.VocabularyId == id)
                .AsNoTracking()
                .ToListAsync();

            return new
            {
                Vocabulary = vocabulary,
                Properties = properties
            };
        }

        public async Task<bool> IsLabelUniqueAsync(string label)
        {
            return await _context.Vocabularies
                .AsNoTracking()
                .AllAsync(v => v.Label != label);
        }

        public async Task<bool> IsNamespaceUriUniqueAsync(string uri)
        {
            return await _context.Vocabularies
                .AsNoTracking()
                .AllAsync(v => v.NamespaceUri != uri);
        }

        public async Task<LMS.Domain.Entities.Property> AddPropertyAync(int vocabularyId, string localName, string label, string termUri)
        {

            bool isDuplicate = await IsPropertyExistsInVocabularyAsync(vocabularyId, localName);
            bool isLabelConflict = !await IsLabelUniqueAsync(label);
            bool isUriConflict = !await IsNamespaceUriUniqueAsync(termUri);

            if (isDuplicate || isLabelConflict || isUriConflict)
            {
                throw new Exception("Property validation failed: Label, URI, or LocalName already exists in system vocabularies.");
            }

            var newProp = new Property
            {
                VocabularyId = vocabularyId,
                LocalName = localName,
                Label = label,
                TermUri = termUri
            };

            var entry = await _context.Properties.AddAsync(newProp);
            return entry.Entity;
        }

        public async Task<bool> IsPropertyExistsInVocabularyAsync(int vocabularyId, string localName)
        {
            return await _context.Properties
                .AsNoTracking()
                .AnyAsync(p => p.VocabularyId == vocabularyId && p.LocalName == localName);
        }
        public async Task<bool> HasLinkedValuesAsync(int propertyId)
        {
            return await _context.Values.AnyAsync(v => v.PropertyId == propertyId);
        }
        public async Task<Property> DeletePropertyAync(int propertyId)
        {
            var property = await _context.Properties
                .FirstOrDefaultAsync(p => p.Id == propertyId);

            if (property == null)
            {
                return null;
            }
            _context.Properties.Remove(property);

            return property;
        }

        
public async Task<Property> UpdatePropertyAync(int propertyId, string localName, string label, string TermUri)
{
    var property = await _context.Properties
        .FirstOrDefaultAsync(p => p.Id == propertyId);

    if (property == null)
    {
        return null; 
    }

    property.LocalName = localName;
    property.Label = label;
    property.TermUri = TermUri;

    _context.Properties.Update(property);

    return property;
}
    }
}