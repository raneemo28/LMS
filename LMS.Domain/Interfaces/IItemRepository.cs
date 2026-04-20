using LMS.Domain.Entities;
using System.Linq.Expressions;

namespace LMS.Domain.Interfaces
{
    public interface IItemRepository : IResourceRepository<Item>
    {
        Task<Item?> GetItemWithFullDataAsync(int id);
        Task<IEnumerable<Item>> GetItemsWithFullDataWithConditionAsync(Expression<Func<Item, bool>> filter);
    }
}