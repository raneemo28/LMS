using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.infra.Database;
using Microsoft.EntityFrameworkCore;

namespace LMS.infra.Repository
{
    public class ResourceTemplateRepository : GenericRepository<ResourceTemplate>, IResourceTemplateRepository
    {
        public ResourceTemplateRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<ResourceTemplate?> GetTemplateWithPropertiesAsync(int id)
        {
            return await _context.ResourceTemplates
            .Include(t=> t.)
                .Include(t => t.Properties)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> IsLabelUniqueAsync(string label)
        {
            return await _context.ResourceTemplates
                .AsNoTracking()
                .AllAsync(t => t.Label != label);
        }
    }
}