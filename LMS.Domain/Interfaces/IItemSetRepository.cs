using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces;
public interface IItemSetRepository : IResourceRepository
{
    Task<IEnumerable<ItemSet>> GetPublicSetsAsync();

    Task<Object?> GetSetWithMembersAsync(int setId);

}