using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using FFMpegCore;
using LMS.App.Interface;

namespace LMS.Infrastructure.ServicesStorage
{
    public class MediaProcessingService : IMediaProcessingService
    {
        public async Task<byte[]?> GenerateThumbnailAsync(byte[] content, string mimeType, string filePath)
        {
            try
            {
                if (mimeType.StartsWith("image/"))
                {
                    return await GenerateImageThumbnailAsync(content);
                }
                else if (mimeType.StartsWith("video/"))
                {
                    return await GenerateVideoThumbnailAsync(filePath);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        // 📸 Image Thumbnail
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

        // 🎥 Video Thumbnail
        private async Task<byte[]?> GenerateVideoThumbnailAsync(string videoPath)
        {
            try
            {
                var tempThumbPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.jpg");

                await FFMpeg.SnapshotAsync(
                    videoPath,
                    tempThumbPath,
                    new System.Drawing.Size(150, 150),
                    TimeSpan.FromSeconds(1)
                );

                if (File.Exists(tempThumbPath))
                {
                    var bytes = await File.ReadAllBytesAsync(tempThumbPath);
                    File.Delete(tempThumbPath);
                    return bytes;
                }
            }
            catch { }

            return null;
        }
    }
}