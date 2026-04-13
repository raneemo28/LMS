using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces;
public interface IItemSetRepository : IResourceRepository
{
    Task<Item> AddItemToSetAsync(int setId, int itemId);
    Task<Item> RemoveItemFromSetAsync(int setId, int itemId);

    Task<Object?> GetSetWithMembersAsync(int setId);

}