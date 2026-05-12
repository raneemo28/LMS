using LMS.Domain.Constants;
using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infra.Repository
{
    public class ResourceTemplateRepository : GenericRepository<ResourceTemplate>, IResourceTemplateRepository
    {
        public ResourceTemplateRepository(LibraryDbContext context) : base(context)
        {
        }
        public async Task<ResourceTemplate?> GetTemplateWithPropertiesAsync(int id)
        {
            return await _context.ResourceTemplates
                .Include(t => t.TemplateProperties)
                    .ThenInclude(tp => tp.Property)
                        .ThenInclude(p => p.Vocabulary)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> IsLabelUniqueAsync(string label)
        {
            return await _context.ResourceTemplates
                .AsNoTracking()
                .AllAsync(t => t.Label != label);
        }

        public async Task<ResourceTemplate> AddPropertyToTemplateAsync(int templateId,int propertyId,bool isRequired,int displayOrder,string? alternateLabel)
        {
            var template = await _context.ResourceTemplates.FindAsync(templateId);
            if (template == null) return null!;
            var property = await _context.Properties.FindAsync(propertyId);
            if (property == null) return null!;
            var link = new TemplateProperty
            {
                TemplateId = templateId,
                PropertyId = propertyId,
                IsRequired = isRequired,
                DisplayOrder = displayOrder,
                AlternateLabel = alternateLabel
            };

            await _context.TemplateProperties.AddAsync(link);

            return template;
        }
        public async Task<ResourceTemplate> RemovePropertyFromTemplateAsync(int templateId, int propertyId)
        {
            var template = await _context.ResourceTemplates.FindAsync(templateId);
            if (template == null) return null;
            var link = await _context.TemplateProperties
                .FirstOrDefaultAsync(tp => tp.TemplateId == templateId && tp.PropertyId == propertyId);

            if (link != null)
            {
                _context.TemplateProperties.Remove(link);
                return template;
            }
            return null;
        }
        public async Task<ResourceTemplate> UpdatePropertyInTemplateAsync(int templateId, int propertyId,bool isRequired,int displayOrder,string? alternateLabel)
        {
            var template = await _context.ResourceTemplates.FindAsync(templateId);
            var propertyLink = await _context.TemplateProperties
             .FirstOrDefaultAsync(p => p.TemplateId == templateId && p.PropertyId == propertyId);
            if (template == null || propertyLink == null) return null!;
            propertyLink.IsRequired=isRequired;
            propertyLink.DisplayOrder=displayOrder;
            propertyLink.AlternateLabel=alternateLabel;
            _context.TemplateProperties.Update(propertyLink);

            return template;
        }
    }
}
