using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.infra.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LMS.infra.Repository
{
    public class ItemRepository : ResourceRepository<Item>, IItemRepository
    {
        public ItemRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Item>> GetItemsWithFullDataWithConditionAsync(Expression<Func<Item, bool>> filter)
        {
            return await _context.Items
            .Include(i => i.Template)
            .Include(i => i.Values)
            .ThenInclude(p => p.Property)
            .ThenInclude(v => v.Vocabulary)
            .AsNoTracking()
            .Where(filter)
            .ToListAsync();
        }

        public async Task<Item?> GetItemWithFullDataAsync(int id)
        {
            return await _context.Items
                .Include(i => i.Template)
                .Include(i => i.Values)
                .ThenInclude(p => p.Property)
                .ThenInclude(v => v.Vocabulary)
                .AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}