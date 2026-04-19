using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
    public interface IResourceRepository : IGenericRepository<Resource>
    {

        Task<bool> UpdateValueAsync(int resourceId, int valueId, string? valueText, string? valueUri, int? valueResourceId, string type, string? language);
        Task<bool> RemoveValueAsync(int resourceId, int valueId);
        Task<IEnumerable<Value>> GetResourceValuesAsync(int resourceId);
        Task<string> GetResourceTypeAsync(int resourceId);
        Task<IEnumerable<Resource>> GetResourcesByTypeAsync(string typeName);
        Task<Value> AddValueAsync(int resourceId, int propertyId, string? valueText, string? valueUri, int? valueResourceId, string type, string? language);
        Task<bool> IsOwnerAsync(int setId, string userId);
        Task<bool> IsPropertyValidForItem(int propertyId, int itemId);
        Task<bool> IsPropertyRequieredForItem(int propertyId,int ItemId);
    }
}