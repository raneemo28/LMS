using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
public interface IMediaRepository : IResourceRepository
    {
        Task<IEnumerable<Media>> GetMediaByItemIdAsync(int itemId);
    }
}