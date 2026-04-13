using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
    public interface IMediaRepository : IResourceRepository
    {
        Task<(Stream fileStream, string contentType, string fileName)> DownloadMediaAsync(int mediaId);
        Task<string> UploadMediaAsync(int mediaId, byte[] fileContent, string fileName, string mimeType);
        Task<IEnumerable<Media>> GetMediaByItemIdAsync(int itemId);
    }
}