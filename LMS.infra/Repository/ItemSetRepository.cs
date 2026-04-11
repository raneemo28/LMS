using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.infra.Database;
using Microsoft.EntityFrameworkCore;

namespace LMS.infra.Repository
{
    public class ItemSetRepository : ResourceRepository, IItemSetRepository
    {
        public ItemSetRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ItemSet>> GetPublicSetsAsync()
        {
            return await _context.ItemSets
                .Where(s => s.IsPublic)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<object?> GetSetWithMembersAsync(int setId)
        {
            // 1. جلب المجموعة أولاً
            var itemSet = await _context.ItemSets
                .Include(s => s.Values)
                    .ThenInclude(v => v.Property)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == setId);

            if (itemSet == null) return null;

            var allItems = await _context.Items
                .Include(i => i.Template)
                .Include(i => i.Values)
                    .ThenInclude(v => v.Property)
                .AsNoTracking()
                .ToListAsync();
            var members = allItems.Where(i =>
            {
                var entry = _context.Entry(i);
                return entry.Property("ItemSetId").CurrentValue != null &&
                    (int)entry.Property("ItemSetId").CurrentValue == setId;
            }).ToList();

            return new
            {
                Set = itemSet,
                Members = members
            };
        }
    }
}