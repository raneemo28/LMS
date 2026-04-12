using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.infra.Database;
using Microsoft.EntityFrameworkCore;
using LMS.Domain.Constants;

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
        var itemSet = await _context.ItemSets
            .Include(s => s.Values)
                .ThenInclude(v => v.Property)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == setId);

        if (itemSet == null) return null;

        var members = await _context.Items
            .Where(i => i.Values.Any(v =>
                v.Property.TermUri == SystemConstants.IsMemberOf &&
                v.ValueText == setId.ToString()))
            .Include(i => i.Template)
            .Include(i => i.Values)
                .ThenInclude(v => v.Property)
            .AsNoTracking()
            .ToListAsync();

        return new
        {
            SetInfo = itemSet,
            Members = members
        };
    }
}
}