using LMS.App.Interface;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace LMS.Infra.ServiceStorage;

public class MediaProcessingService : IMediaProcessingService
{
    public async Task<byte[]?> GenerateThumbnailAsync(byte[] content, string mimeType, string filePath)
    {
        if (!mimeType.StartsWith("image/")) return null;
        using var inStream = new MemoryStream(content);
        using var image = await Image.LoadAsync(inStream);
        image.Mutate(x => x.Resize(new ResizeOptions { 
            Size = new SixLabors.ImageSharp.Size(150, 150), 
            Mode = ResizeMode.Max }));
        using var outStream = new MemoryStream();
        await image.SaveAsJpegAsync(outStream);
        return outStream.ToArray();
    }
}
