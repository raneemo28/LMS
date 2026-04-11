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
    }
}