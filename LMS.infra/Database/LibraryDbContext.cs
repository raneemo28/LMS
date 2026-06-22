using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infra.Database;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

    public DbSet<Vocabulary> Vocabularies { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<ResourceTemplate> ResourceTemplates { get; set; }
    public DbSet<TemplateProperty> TemplateProperties { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemSet> ItemSets { get; set; }
    public DbSet<Media> Medias { get; set; }
    public DbSet<Value> Values { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vocabulary>(entity =>
        {
            entity.ToTable("VOCABULARY");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Prefix).IsRequired().HasMaxLength(50);
            entity.Property(e => e.NamespaceUri).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Label).IsRequired().HasMaxLength(255);

            entity.HasData(new Vocabulary
            {
                Id = 1,
                Prefix = "sys",
                NamespaceUri = "http://schema.lms.com/system#",
                Label = "System Internal Vocabulary"
            });
        });

        modelBuilder.Entity<ResourceTemplate>(entity =>
        {
            entity.ToTable("RESOURCE_TEMPLATE");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Label).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.ToTable("PROPERTY");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LocalName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Label).IsRequired().HasMaxLength(255);
            entity.Property(e => e.TermUri).IsRequired().HasMaxLength(255);
            entity.HasOne(d => d.Vocabulary).WithMany().HasForeignKey(d => d.VocabularyId);

            entity.HasData(
                new Property
                {
                    Id = 1,
                    Label = "Associated Media",
                    LocalName = "hasMedia",
                    TermUri = "http://schema.lms.com/system#hasMedia",
                    VocabularyId = 1
                },
                new Property
                {
                    Id = 2,
                    Label = "Thumbnail",
                    LocalName = "hasThumbnail",
                    TermUri = "http://schema.lms.com/system#hasThumbnail",
                    VocabularyId = 1
                },
                new Property
               {
            Id = 3,
            Label = "Is Member Of",
            LocalName = "isMemberOf",
            TermUri = "http://schema.lms.com/system#isMemberOf",
            VocabularyId = 1
            }
            );
        });

        modelBuilder.Entity<TemplateProperty>(entity =>
        {
            entity.ToTable("TEMPLATE_PROPERTY");
            entity.HasKey(e => new { e.TemplateId, e.PropertyId });
            entity.HasOne(d => d.Template).WithMany(t => t.TemplateProperties).HasForeignKey(d => d.TemplateId);
            entity.HasOne(d => d.Property).WithMany().HasForeignKey(d => d.PropertyId);
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.ToTable("RESOURCE");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.OwnerId).HasMaxLength(450);
        });

        modelBuilder.Entity<ItemSet>(entity =>
        {
            entity.ToTable("ITEM_SET");
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("ITEM");
            entity.HasOne(d => d.Template).WithMany().HasForeignKey(d => d.TemplateId);
        });

        modelBuilder.Entity<Media>(entity =>
        {
            entity.ToTable("MEDIA");
            entity.Property(e => e.StoragePath).IsRequired();
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.HasOne(d => d.Item).WithMany().HasForeignKey(d => d.ItemId);
        });

        modelBuilder.Entity<Value>(entity =>
        {
            entity.ToTable("VALUE");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Language).HasMaxLength(10);

            entity.HasOne(d => d.Resource)
                .WithMany(p => p.Values)
                .HasForeignKey(d => d.ResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Property)
                .WithMany()
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(d => d.ValueResource)
                .WithMany()
                .HasForeignKey(d => d.ValueResourceId)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
