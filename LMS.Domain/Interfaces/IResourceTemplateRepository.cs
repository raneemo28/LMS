using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
    public interface IResourceTemplateRepository : IGenericRepository<ResourceTemplate>
    {
        Task<ResourceTemplate?> GetTemplateWithPropertiesAsync(int id);
        Task<bool> IsLabelUniqueAsync(string label);
    }
}