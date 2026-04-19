using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
public interface IResourceRepository<T> : IGenericRepository<T> where T : Resource
    {

        Task<bool> AddResourceWithValue(Resource resource, Value value);

        Task<bool> UpdateResourceWithValue(int resourceId, Value value);

        Task<bool> RemoveResourceWithValue(int resourceId, int valueId);

        Task<IEnumerable<Value>> GetResourceValuesAsync(int resourceId);
        Task<Resource> GetResourceTypeAsync(int resourceId);
        Task<IEnumerable<Resource>> GetResourcesByTypeAsync(string typeName);
        Task<Value> AddValueAsync(int resourceId, int propertyId, string? valueText, string? valueUri, int? valueResourceId, string type, string? language);
        Task<bool> IsOwnerAsync(int setId, string userId);
    }
}