using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.infra.Database;
using Microsoft.EntityFrameworkCore;

namespace LMS.infra.Repository
{
    public class MediaRepository : ResourceRepository, IMediaRepository
    {
        public MediaRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Media>> GetMediaByItemIdAsync(int itemId)
        {
            return await _context.Medias
            .Where(m => m.ItemId == itemId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}