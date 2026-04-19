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
        public async Task<ItemSetWithMembers?> GetSetWithMembersAsync(int setId)
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

            return new ItemSetWithMembers(itemSet, members);
        }
        public async Task<Item> AddItemToSetAsync(int setId, int itemId)
        {
            //still need to check the duplication 
            var item = await _context.Items.FindAsync(itemId);
            if (item == null) return null!;

            var memberOfProperty = await _context.Properties
                .FirstOrDefaultAsync(p => p.TermUri == SystemConstants.IsMemberOf);

            if (memberOfProperty == null)
            {
                throw new InvalidOperationException("Property 'IsMemberOf' not defined in vocabularies.");
            }

            var memberValue = new Value
            {
                ResourceId = itemId,
                PropertyId = memberOfProperty.Id,
                ValueText = setId.ToString(),
            };

            await _context.Values.AddAsync(memberValue);

            return item;
        }
        public async Task<Item> RemoveItemFromSetAsync(int setId, int itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item == null) return null!;

            var linkValue = await _context.Values
                .FirstOrDefaultAsync(v =>
                    v.ResourceId == itemId &&
                    v.Property.TermUri == SystemConstants.IsMemberOf &&
                    v.ValueText == setId.ToString());

            if (linkValue != null)
            {
                _context.Values.Remove(linkValue);
            }

            return item;
        }
    }
}
