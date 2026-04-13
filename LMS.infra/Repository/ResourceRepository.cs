using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using LMS.infra.Database;

namespace LMS.infra.Repository
{
    public class ResourceRepository : GenericRepository<Resource>, IResourceRepository
    {
        public ResourceRepository(LibraryDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<T>> GetResourcesByTypeAsync<T>() where T : Resource
        {
            return await _context.Set<T>()
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<IEnumerable<Resource>> GetResourcesByTypeNameAsync(string typeName)
        {
            return await _context.Resources
                .Where(r => r.Type == typeName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Resource> GetResourceTypeAsync(int resourceId)
        {
            return await _context.Resources
            .AsNoTracking()
            .FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Resource not found");
        }

        public async Task<IEnumerable<Value>> GetResourceValuesAsync(int resourceId)
        {
            return await _context.Values
                .Where(v => v.ResourceId == resourceId)
                .Include(v => v.Property)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<bool> IsOwnerAsync(int setId, string userId)
        {
            return await _context.Resources
                .AsNoTracking()
                .AnyAsync(s => s.Id == setId && s.CreatedBy == userId);
        }

        public async Task<Value> AddValueAync(int resourceId, int propertyId, string? valueText, string? valueUri, int? valueResourceId, string type, string? language)
        {
            // Perform validation similar to AddPropertyAync
            bool isResourceValid = await ResourceExistsAsync(resourceId);
            bool isPropertyValid = await PropertyExistsAsync(propertyId);
            bool isDuplicate = await IsValueDuplicateAsync(resourceId, propertyId, valueText, valueUri, valueResourceId, language);

            if (!isResourceValid)
            {
                throw new Exception("Value validation failed: Target Resource does not exist.");
            }

            if (!isPropertyValid)
            {
                throw new Exception("Value validation failed: Referenced Property does not exist.");
            }

            if (isDuplicate)
            {
                throw new Exception("Value validation failed: This specific value already exists for this property on the target resource.");
            }

            var newValue = new Value
            {
                ResourceId = resourceId,
                PropertyId = propertyId,
                ValueText = valueText,
                ValueUri = valueUri,
                ValueResourceId = valueResourceId,
                Type = type,
                Language = language
            };

            var entry = await _context.Values.AddAsync(newValue);
            return entry.Entity;
        }

        public async Task<bool> ResourceExistsAsync(int resourceId)
        {
            return await _context.Resources
                .AsNoTracking()
                .AnyAsync(r => r.Id == resourceId);
        }

        public async Task<bool> PropertyExistsAsync(int propertyId)
        {
            return await _context.Properties
                .AsNoTracking()
                .AnyAsync(p => p.Id == propertyId);
        }

        public async Task<bool> IsValueDuplicateAsync(int resourceId, int propertyId, string? text, string? uri, int? resId, string? lang)
        {
            return await _context.Values
                .AsNoTracking()
                .AnyAsync(v =>
                    v.ResourceId == resourceId &&
                    v.PropertyId == propertyId &&
                    v.ValueText == text &&
                    v.ValueUri == uri &&
                    v.ValueResourceId == resId &&
                    v.Language == lang);
        }


    }
}