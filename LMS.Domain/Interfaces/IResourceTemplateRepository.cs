using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
    public interface IResourceTemplateRepository : IGenericRepository<ResourceTemplate>
    {
        Task<ResourceTemplate> AddPropertyToTemplateAsync(int templateId, string localName, string label, string termUri);
        Task<ResourceTemplate> RemovePropertyFromTemplateAsync(int templateId, int propertyId);
        Task<ResourceTemplate> UpdatePropertyInTemplateAsync(int templateId, int propertyId, string localName, string label, string termUri);
        Task<Object?> GetTemplateWithPropertiesAsync(int id);
        Task<bool> IsLabelUniqueAsync(string label);
    }
}