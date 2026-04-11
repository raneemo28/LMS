using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
    public interface IResourceTemplateRepository : IGenericRepository<ResourceTemplate>
    {
        Task<Object?> GetTemplateWithPropertiesAsync(int id);
        Task<bool> IsLabelUniqueAsync(string label);
    }
}