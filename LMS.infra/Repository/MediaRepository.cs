using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.Infra.Database;
using Microsoft.EntityFrameworkCore;

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
                .Include(m => m.Values)
                    .ThenInclude(v => v.Property)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Media>> GetMediaByOwnerAsync(string ownerId)
        {
            return await _context.Medias
                .Where(m => m.OwnerId == ownerId)
                .Include(m => m.Values)
                    .ThenInclude(v => v.Property)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Media>> GetMediaByMimeTypeAsync(string mimeType)
        {
            return await _context.Medias
                .Where(m => m.MimeType == mimeType)
                .Include(m => m.Values)
                    .ThenInclude(v => v.Property)
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
