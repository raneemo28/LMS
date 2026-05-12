using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.Infra.Database;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace LMS.Infra.Repository
{
    public class MediaRepository : ResourceRepository<Media>, IMediaRepository
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

        public async Task<Media?> GetMediaWithMetadataAsync(int mediaId)
        {
            return await _context.Medias
                .AsNoTracking()
                .Include(m => m.Values)
                    .ThenInclude(v => v.Property) 
                .FirstOrDefaultAsync(m => m.Id == mediaId);
        }
    }
}
