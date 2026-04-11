using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
    public interface IItemRepository : IResourceRepository
    {
        Task<Item?> GetItemWithFullDataAsync(int id);
    }
}