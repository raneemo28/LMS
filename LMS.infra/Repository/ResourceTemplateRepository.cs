using LMS.Domain.Constants;
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


        public async Task<object?> GetTemplateWithPropertiesAsync(int id)
        {
            var template = await _context.ResourceTemplates
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (template == null) return null;

            var properties = await (from tp in _context.Set<TemplateProperty>()
                                    join p in _context.Properties on tp.PropertyId equals p.Id
                                    where tp.TemplateId == id
                                    select new
                                    {
                                        p.Id,
                                        p.LocalName,
                                        p.Label,
                                        p.TermUri,
                                        tp.IsRequired,
                                        tp.DisplayOrder,
                                        tp.AlternateLabel
                                    })
                                    .AsNoTracking()
                                    .OrderBy(x => x.DisplayOrder)
                                    .ToListAsync();

            return new
            {
                TemplateId = template.Id,
                TemplateLabel = template.Label,
                Properties = properties
            };
        }

        public async Task<bool> IsLabelUniqueAsync(string label)
        {
            return await _context.ResourceTemplates
                .AsNoTracking()
                .AllAsync(t => t.Label != label);
        }
    }
}