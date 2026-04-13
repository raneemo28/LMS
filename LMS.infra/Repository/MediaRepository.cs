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
            if (!Directory.Exists(_uploadsFolder)) Directory.CreateDirectory(_uploadsFolder);
            var media = await _context.Set<Media>().FindAsync(mediaId);
            if (media == null) return null!;

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
                    var thumbPath = Path.Combine(_uploadsFolder, "thumb_" + uniqueFileName + ".jpg");
                    await File.WriteAllBytesAsync(thumbPath, thumbnailBytes);
                }
            }
            catch { /*ignore*/ }

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
            var tempThumbPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.jpg");

            await FFMpeg.SnapshotAsync(videoPath, tempThumbPath, new System.Drawing.Size(150, 150), TimeSpan.FromSeconds(1));

            if (File.Exists(tempThumbPath))
            {
                var bytes = await File.ReadAllBytesAsync(tempThumbPath);

                File.Delete(tempThumbPath);

                return bytes;
            }

            return null!;
        }
    }
}