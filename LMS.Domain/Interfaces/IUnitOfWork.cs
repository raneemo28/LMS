using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IResourceRepository<Resource> Resources { get; }
        IItemRepository Items { get; }
        IMediaRepository Media { get; }
        IVocabularyRepository Vocabularies { get; }
        IItemSetRepository ItemSets { get; }
        IResourceTemplateRepository ResourceTemplates { get; }

        Task<int> CommitAsync();
        Task RollbackAsync();
    }
}