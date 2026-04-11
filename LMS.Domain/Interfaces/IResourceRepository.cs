using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
    public interface IResourceRepository : IGenericRepository<Resource>
    {
        Task<IEnumerable<Value>> GetResourceValuesAsync(int resourceId);
        Task<Resource> GetResourceTypeAsync(int resourceId);
        Task<IEnumerable<T>> GetResourcesByTypeAsync<T>() where T : Resource;
        Task<IEnumerable<Resource>> GetResourcesByTypeNameAsync(string typeName);
        Task<bool> IsOwnerAsync(int setId, string userId);
    }
}