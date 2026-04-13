using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.infra.Database;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using FFMpegCore;

namespace LMS.infra.Repository
{
    public class MediaRepository : ResourceRepository, IMediaRepository
    {
        private readonly string _uploadsFolder;

        public MediaRepository(LibraryDbContext context) : base(context)
        {
            _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(_uploadsFolder)) Directory.CreateDirectory(_uploadsFolder);
        }

        public async Task<IEnumerable<Media>> GetMediaByItemIdAsync(int itemId)
        {
            return await _context.Set<Media>()
                .Where(m => m.ItemId == itemId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<string> UploadMediaAsync(int mediaId, byte[] fileContent, string fileName, string mimeType)
        {
            var media = await _context.Set<Media>().FindAsync(mediaId);
            if (media == null) return null!;

            // 1. Save the original file
            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var fullPath = Path.Combine(_uploadsFolder, uniqueFileName);
            await File.WriteAllBytesAsync(fullPath, fileContent);

            try
            {
                byte[]? thumbnailBytes = null;
                if (mimeType.StartsWith("image/"))
                {
                    thumbnailBytes = await GenerateImageThumbnailAsync(fileContent);
                }
                else if (mimeType.StartsWith("video/"))
                {
                    thumbnailBytes = await GenerateVideoThumbnailAsync(fullPath);
                }

                if (thumbnailBytes != null)
                {
                    var thumbPath = Path.Combine(_uploadsFolder, "thumb_" + uniqueFileName);
                    await File.WriteAllBytesAsync(thumbPath, thumbnailBytes);
                }
            }
            catch
            {
            }

            // 3. Update entity data
            media.FileName = fileName;
            media.StoragePath = $"/uploads/{uniqueFileName}";
            media.MimeType = mimeType;
            media.FileSize = fileContent.Length;

            _context.Set<Media>().Update(media);

            // Return the path as defined in the Interface
            return media.StoragePath;
        }

        public async Task<(Stream fileStream, string contentType, string fileName)> DownloadMediaAsync(int mediaId)
        {
            // 1. Get metadata from database
            var media = await _context.Set<Media>()
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == mediaId);

            if (media == null || string.IsNullOrEmpty(media.StoragePath))
                return (null!, null!, null!);

            var relativePath = media.StoragePath.TrimStart('/');
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

            if (!File.Exists(fullPath))
                return (null!, null!, null!);

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);

            return (stream, media.MimeType ?? "application/octet-stream", media.FileName);
        }

        private async Task<byte[]> GenerateImageThumbnailAsync(byte[] content)
        {
            using var inStream = new MemoryStream(content);
            using var image = await Image.LoadAsync(inStream);

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(150, 150),
                Mode = ResizeMode.Max
            }));

            using var outStream = new MemoryStream();
            await image.SaveAsJpegAsync(outStream);
            return outStream.ToArray();
        }

        private async Task<byte[]> GenerateVideoThumbnailAsync(string videoPath)
        {
            try
            {
                var tempThumbPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.jpg");
                await FFMpeg.SnapshotAsync(videoPath, tempThumbPath, new System.Drawing.Size(150, 150), TimeSpan.FromSeconds(1));

                if (File.Exists(tempThumbPath))
                {
                    var bytes = await File.ReadAllBytesAsync(tempThumbPath);
                    File.Delete(tempThumbPath);
                    return bytes;
                }
            }
            catch { }
            return null!;
        }
    }
}