using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces;
public interface IItemSetRepository : IResourceRepository<ItemSet>
{
    Task<Item> AddItemToSetAsync(int setId, int itemId);
    Task<Item> RemoveItemFromSetAsync(int setId, int itemId);
    Task<IEnumerable<ItemSet>> GetPublicOrOwnedAsync(string userId);
    Task<ItemSetWithMembers?> GetSetWithMembersAsync(int setId);
}
