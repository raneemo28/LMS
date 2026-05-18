using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
    public interface IMediaRepository : IResourceRepository<Media>
    {
        Task<IEnumerable<Media>> GetMediaByItemIdAsync(int itemId);
        Task<IEnumerable<Media>> GetMediaByOwnerAsync(string ownerId);
        Task<IEnumerable<Media>> GetMediaByMimeTypeAsync(string mimeType);
        Task<Media?> GetMediaWithMetadataAsync(int mediaId);
    }
}