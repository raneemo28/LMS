using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.Infra.Database;
using Microsoft.EntityFrameworkCore;
using LMS.Domain.Constants;

namespace LMS.Infra.Repository
{
    public class ItemSetRepository : ResourceRepository<ItemSet>, IItemSetRepository
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

            // OPTIMIZATION: 
            // Instead of joining Item -> Value -> Property to check TermUri,
            // we query the Value table directly using our new composite index (PropertyId, ValueText).
            // First, get the PropertyId for 'isMemberOf' (this hits the unique index on Property.TermUri)
            var isMemberOfPropertyId = await _context.Properties
                .Where(p => p.TermUri == SystemConstants.IsMemberOfUri)
                .Select(p => p.Id)
                .FirstOrDefaultAsync();

            // Then, find the ResourceIds (Items) that have this PropertyId and ValueText
            var memberResourceIds = await _context.Values
                .Where(v => v.PropertyId == isMemberOfPropertyId && v.ValueText == setId.ToString())
                .Select(v => v.ResourceId)
                .ToListAsync();

            // Finally, fetch the actual Items
            var members = await _context.Items
                .Where(i => memberResourceIds.Contains(i.Id))
                .Include(i => i.Template)
                .Include(i => i.Values)
                    .ThenInclude(v => v.Property)
                .AsNoTracking()
                .ToListAsync();

            return new ItemSetWithMembers(itemSet, members);
        }

        public async Task<IEnumerable<ItemSet>> GetPublicOrOwnedAsync(string userId)
        {
            return await _context.ItemSets
                .Where(s => s.IsPublic || s.OwnerId == userId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Item> AddItemToSetAsync(int setId, int itemId)
        {
            //still need to check the duplication 
            var item = await _context.Items.FindAsync(itemId);
            if (item == null) return null!;

            var memberOfProperty = await _context.Properties
                .FirstOrDefaultAsync(p => p.TermUri == SystemConstants.IsMemberOfUri);

            if (memberOfProperty == null)
            {
                throw new InvalidOperationException("Property 'IsMemberOf' not defined in vocabularies.");
            }

            var memberValue = new Value
            {
                ResourceId = itemId,
                PropertyId = memberOfProperty.Id,
                ValueText = setId.ToString(),
                Type = SystemConstants.TypeText   // "text"
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
                    v.Property.TermUri == SystemConstants.IsMemberOfUri &&
                    v.ValueText == setId.ToString());

            if (linkValue != null)
            {
                _context.Values.Remove(linkValue);
            }

            return item;
        }
    }
}
