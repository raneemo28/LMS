using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
    public interface IMediaRepository : IResourceRepository
    {
        Task<Media> DownloadMediaAsync(int mediaId);
        Task<Media> UploadMediaAsync(int mediaId, byte[] fileContent, string fileName, string mimeType);
        Task<IEnumerable<Media>> GetMediaByItemIdAsync(int itemId);
    }
}