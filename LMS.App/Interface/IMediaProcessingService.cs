namespace LMS.App.Interface;
public interface IMediaProcessingService
{
    Task<byte[]?> GenerateThumbnailAsync(byte[] content, string mimeType, string filePath);
}
