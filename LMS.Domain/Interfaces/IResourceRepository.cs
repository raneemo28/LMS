using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
public interface IResourceRepository<T> : IGenericRepository<T> where T : Resource
    {

        void UpdateValue(Value value);
        Task<bool> RemoveValueAsync(int resourceId, int valueId);
        Task<IEnumerable<Value>> GetResourceValuesAsync(int resourceId);
        Task<string> GetResourceTypeAsync(int resourceId);
        Task<Value> AddValueAsync(int resourceId, int propertyId, string? valueText, string? valueUri, int? valueResourceId, string type, string? language);
        Task<bool> IsOwnerAsync(int resourceId, string userId);
        Task<bool> IsPropertyValidForItem(int propertyId, int itemId);
        Task<bool> IsPropertyRequiredForItem(int propertyId, int itemId);
        Task<Value?> GetValueByIdAsync(int valueId, int resourceId);
    }
}