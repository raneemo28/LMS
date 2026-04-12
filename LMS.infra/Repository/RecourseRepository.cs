using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using LMS.infra.Database;
{
    
}
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
                .ToListAsync();        }

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
    }
}