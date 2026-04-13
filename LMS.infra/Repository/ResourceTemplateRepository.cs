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

        public async Task<ResourceTemplate> AddPropertyToTemplateAsync(int templateId, string localName, string label, string termUri)
        {
            var template = await _context.ResourceTemplates.FindAsync(templateId);
            if (template == null) return null!;

            var newProperty = new Property
            {
                LocalName = localName,
                Label = label,
                TermUri = termUri
            };
            await _context.Properties.AddAsync(newProperty);


            var link = new TemplateProperty
            {
                TemplateId = templateId,
                Property = newProperty,
                IsRequired = false,
                DisplayOrder = 0
            };

            await _context.Set<TemplateProperty>().AddAsync(link);

            return template;
        }
        public async Task<ResourceTemplate> RemovePropertyFromTemplateAsync(int templateId, int propertyId)
        {
            var template = await _context.ResourceTemplates.FindAsync(templateId);

            var link = await _context.Set<TemplateProperty>()
                .FirstOrDefaultAsync(tp => tp.TemplateId == templateId && tp.PropertyId == propertyId);

            if (link != null)
            {
                _context.Set<TemplateProperty>().Remove(link);

                var property = await _context.Properties.FindAsync(propertyId);
                if (property != null) _context.Properties.Remove(property);

                return template!;
            }

            return null!;
        }
        public async Task<ResourceTemplate> UpdatePropertyInTemplateAsync(int templateId, int propertyId, string localName, string label, string termUri)
        {
            var template = await _context.ResourceTemplates.FindAsync(templateId);
            var property = await _context.Properties
                .FirstOrDefaultAsync(p => p.Id == propertyId);

            if (template == null || property == null) return null!;

            property.LocalName = localName;
            property.Label = label;
            property.TermUri = termUri;

            _context.Properties.Update(property);

            return template;
        }
    }
}