using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
    public interface IMediaRepository : IResourceRepository<Media>
    {
        Task<IEnumerable<Media>> GetMediaByItemIdAsync(int itemId);
        Task<Media?> GetMediaWithMetadataAsync(int mediaId);
    }
}