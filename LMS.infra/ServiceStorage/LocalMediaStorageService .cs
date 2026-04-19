using LMS.App.Interface;

namespace LMS.Infra.ServiceStorage;

public class LocalMediaStorageService : IMediaStorageService
{
    private readonly string _basePath;

    public LocalMediaStorageService()
    {
        _basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
    }

    public async Task<string> UploadAsync(byte[] content, string fileName, string contentType)
    {
        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);

        var filePath = Path.Combine(_basePath, fileName);

        await File.WriteAllBytesAsync(filePath, content);

        return filePath;
    }

    public async Task<(Stream stream, string contentType, string fileName)> DownloadAsync(string path)
{
    if (!File.Exists(path))
        throw new FileNotFoundException("File not found", path);

    var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

    var fileName = Path.GetFileName(path);
    var contentType = "application/octet-stream"; 

    return (stream, contentType, fileName);
}
}