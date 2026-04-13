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

        public async Task<bool> AddResourceWithValue(Resource resource, Value value)
        {
            await _context.Resources.AddAsync(resource);

            value.Resource = resource;
            await _context.Values.AddAsync(value);

            return true;
        }

        public async Task<bool> UpdateResourceWithValue(int resourceId, Value value)
        {
            var existingValue = await _context.Values
                .FirstOrDefaultAsync(v => v.ResourceId == resourceId && v.Id == value.Id);

            if (existingValue == null) return false;

            existingValue.ValueText = value.ValueText;
            existingValue.ValueUri = value.ValueUri;
            existingValue.ValueResourceId = value.ValueResourceId;
            existingValue.Language = value.Language;
            existingValue.Type = value.Type;

            _context.Values.Update(existingValue);
            return true;
        }
        public async Task<bool> RemoveResourceWithValue(int resourceId, int valueId)
        {
            var value = await _context.Values
                .FirstOrDefaultAsync(v => v.Id == valueId && v.ResourceId == resourceId);

            if (value == null) return false;

            _context.Values.Remove(value);
            return true;
        }
    }
}