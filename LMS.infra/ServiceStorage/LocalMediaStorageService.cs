using LMS.App.Interface;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;

namespace LMS.Infra.ServiceStorage;

public class LocalMediaStorageService : IMediaStorageService
{
    private readonly string _basePath;
    private readonly FileExtensionContentTypeProvider _provider = new();

    public LocalMediaStorageService(IConfiguration config)
    {
        var relative = config["Storage:BasePath"] ?? "wwwroot/uploads";
        _basePath = Path.Combine(AppContext.BaseDirectory, relative);
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> UploadAsync(byte[] content, string fileName, string contentType)
    {
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
        _provider.TryGetContentType(fileName, out contentType);

        return (stream, contentType ?? "application/octet-stream", fileName);
    }

    public Task DeleteAsync(string path)
    {
        if (!File.Exists(path))
        {
            return Task.CompletedTask;
        }

        File.Delete(path);
        return Task.CompletedTask;
    }
}
