namespace LMS.App.Interface;
public interface IMediaStorageService
{
    Task<string> UploadAsync(byte[] content, string fileName, string contentType);
    Task<(Stream stream, string contentType, string fileName)> DownloadAsync(string path);
    }
