using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.Infra.Database;

namespace LMS.Infra.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;

        public IResourceRepository<Resource> Resources { get; private set; }
        public IItemRepository Items { get; private set; }
        public IItemSetRepository ItemSets { get; private set; }
        public IVocabularyRepository Vocabularies { get; private set; }
        public IResourceTemplateRepository ResourceTemplates { get; private set; }
        public IMediaRepository Media { get; private set; }

        public UnitOfWork(LibraryDbContext context,
            IResourceRepository<Resource> resources,
            IItemRepository items,
            IItemSetRepository itemSets,
            IVocabularyRepository vocabularies,
            IResourceTemplateRepository templates,
            IMediaRepository media)
        {
            _context = context;
            Resources = resources; Items = items; ItemSets = itemSets;
            Vocabularies = vocabularies; ResourceTemplates = templates; Media = media;
        }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

        public Task RollbackAsync() => Task.CompletedTask; // EF Core auto-rolls back on exception

        public void Dispose() => _context.Dispose();

    }
}
