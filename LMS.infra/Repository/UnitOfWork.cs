using LMS.Domain.Interfaces;
using LMS.infra.Database;

namespace LMS.infra.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;

        public IResourceRepository Resources { get; private set; }
        public IItemRepository Items { get; private set; }
        public IItemSetRepository ItemSets { get; private set; }
        public IVocabularyRepository Vocabularies { get; private set; }
        public IResourceTemplateRepository ResourceTemplates { get; private set; }
        public IMediaRepository Media { get; private set; }

        public UnitOfWork(LibraryDbContext context)
        {
            _context = context;
            Resources = new ResourceRepository(_context);
            Items = new ItemRepository(_context);
            ItemSets = new ItemSetRepository(_context);
            Vocabularies = new VocabularyRepository(_context);
            ResourceTemplates = new ResourceTemplateRepository(_context);
            Media = new MediaRepository(_context);
        }

        public async Task<int> CommitAsync() 
        {
            return await _context.SaveChangesAsync();
        }

        public Task RollbackAsync()
        {
            _context.ChangeTracker.Entries().ToList().ForEach(x => x.State = Microsoft.EntityFrameworkCore.EntityState.Detached);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}