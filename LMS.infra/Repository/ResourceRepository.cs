using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using LMS.Infra.Database;

namespace LMS.Infra.Repository
{
public class ResourceRepository<T> : GenericRepository<T>, IResourceRepository<T> where T : Resource    {
        public ResourceRepository(LibraryDbContext context) : base(context)
        {
        }
        public async Task<string> GetResourceTypeAsync(int resourceId)
        {
            var resource = await _context.Resources
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == resourceId) ?? throw new KeyNotFoundException("Resource not found");
            return resource.Type;
        }

        public async Task<IEnumerable<Value>> GetResourceValuesAsync(int resourceId)
        {
            return await _context.Values
                .Where(v => v.ResourceId == resourceId)
                .Include(v => v.Property)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<bool> IsOwnerAsync(int resourceId, string userId)
        {
            return await _context.Resources.AsNoTracking()
                .AnyAsync(r => r.Id == resourceId && r.OwnerId == userId);
        }

        public async Task<Value> AddValueAsync(int resourceId, int propertyId, string? valueText, string? valueUri, int? valueResourceId, string type, string? language)
        {
            bool isResourceValid = await ResourceExistsAsync(resourceId);
            bool isPropertyValid = await PropertyExistsAsync(propertyId);
            bool isDuplicate = await IsValueDuplicateAsync(resourceId, propertyId, valueText, valueUri, valueResourceId, language);
            if (await GetResourceTypeAsync(resourceId)=="Item")
            {
                bool isPropertyValidForItem = await IsPropertyValidForItem(propertyId, resourceId);
                if (!isPropertyValidForItem)
                {
                    throw new Exception("Value validation failed: Property is not valid for this item.");
                }
            }
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
        public async Task<bool> IsPropertyValidForItem(int propertyId, int itemId)
        {
            var item = await _context.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == itemId);
            if (item == null) return false;
            return await _context.TemplateProperties
                .AsNoTracking()
                .AnyAsync(tp => tp.PropertyId == propertyId && tp.TemplateId == item.TemplateId);
        }
        public async Task<bool> IsPropertyRequiredForItem(int propertyId,int ItemId)
        {
            var item = await _context.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == ItemId);
            if (item == null) return false;
            return await _context.TemplateProperties
                .AsNoTracking()
                .AnyAsync(tp => tp.PropertyId == propertyId && tp.TemplateId == item.TemplateId && tp.IsRequired);
        }
        public async Task<bool> UpdateValueAsync(int resourceId, int valueId, string? valueText, string? valueUri, int? valueResourceId, string type, string? language)
        {
            var value = await _context.Values
                .FirstOrDefaultAsync(v => v.Id == valueId && v.ResourceId == resourceId);
            if (value == null) return false;
            value.ValueText = valueText;
            value.ValueUri = valueUri;
            value.ValueResourceId = valueResourceId;
            value.Type = type;
            value.Language = language;
            return true;
        }

        public async Task<bool> RemoveValueAsync(int resourceId, int valueId)
        {
            var value = await _context.Values
                .FirstOrDefaultAsync(v => v.Id == valueId && v.ResourceId == resourceId);
            if (value == null) return false;
            if(await GetResourceTypeAsync(resourceId)=="Item")
            {
                if(await IsPropertyRequiredForItem(value.PropertyId,resourceId))
                {
                    throw new Exception("Value validation failed: Property is required for this item.");
                }
            }
            _context.Values.Remove(value);
            return true;
        }
    }
}
