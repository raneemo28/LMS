using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
    public interface IResourceTemplateRepository : IGenericRepository<ResourceTemplate>
    {
        Task<ResourceTemplate> AddPropertyToTemplateAsync(int templateId,int propertyId,bool isRequired,int displayOrder,string? alternateLabel);
        Task<ResourceTemplate> RemovePropertyFromTemplateAsync(int templateId, int propertyId);
        Task<ResourceTemplate> UpdatePropertyInTemplateAsync(int templateId, int propertyId, bool isRequired,int displayOrder,string? alternateLabel);
        Task<ResourceTemplate?> GetTemplateWithPropertiesAsync(int id);
        Task<bool> IsLabelUniqueAsync(string label);
    }
}