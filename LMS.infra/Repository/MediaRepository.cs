using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.infra.Database;
using Microsoft.EntityFrameworkCore;

namespace LMS.infra.Repository
{
    public class MediaRepository : ResourceRepository, IMediaRepository
    {
        private readonly string _storagePath;
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
        public async Task<Media> UploadMediaAsync(int mediaId, byte[] fileContent, string fileName, string mimeType)
        {
            if (!Directory.Exists(_storagePath)) Directory.CreateDirectory(_storagePath);
            var media = await _context.Set<Media>().FindAsync(mediaId);

            if (media == null) return null!;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

            await File.WriteAllBytesAsync(fullPath, fileContent);

            media.FileName = fileName;
            media.StoragePath = $"/uploads/{uniqueFileName}";
            media.MimeType = mimeType;
            media.FileSize = fileContent.Length;

            _context.Set<Media>().Update(media);

            return media;
        }

        public async Task<Media?> DownloadMediaAsync(int mediaId)
        {
            return await _context.Set<Media>()
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == mediaId);
        }
    }
}