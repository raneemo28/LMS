using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.infra.Database;
using Microsoft.EntityFrameworkCore;

namespace LMS.infra.Repository
{
    public class ItemRepository : ResourceRepository, IItemRepository
    {
        public ItemRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<Item?> GetItemWithFullDataAsync(int id)
        {
            return await _context.Items
                .Include(i=> i.Template)
                .Include(i => i.Values)
                .ThenInclude(p => p.Property)
                .ThenInclude(v => v.Vocabulary)
                .AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}