using LMS.Domain.Constants;
using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMS.Infra.Database;

public static class DbSeeder
{
    public static async Task SeedSystemMetadataAsync(LibraryDbContext context, ILogger logger)
    {
        // 1. Ensure System Vocabulary exists
        var sysVocab = await context.Vocabularies
            .FirstOrDefaultAsync(v => v.Prefix == SystemConstants.SystemPrefix);
            
        if (sysVocab == null)
        {
            sysVocab = new Vocabulary 
            { 
                Prefix = SystemConstants.SystemPrefix, 
                NamespaceUri = SystemConstants.SystemNamespace,
                Label = SystemConstants.SystemLabel 
            };
            context.Vocabularies.Add(sysVocab);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded System Vocabulary: {Prefix}", SystemConstants.SystemPrefix);
        }

        // 2. Ensure Structural Properties exist
        var propsToSeed = new List<Property>
        {
            new() { LocalName = SystemConstants.IsMemberOf, Label = "Member Of" },
            new() { LocalName = SystemConstants.HasMedia, Label = "Associated Media" },
            new() { LocalName = SystemConstants.HasThumbnail, Label = "Thumbnail" }
        };

        foreach (var prop in propsToSeed)
        {
            var exists = await context.Properties
                .AnyAsync(p => p.LocalName == prop.LocalName && p.VocabularyId == sysVocab.Id);

            if (!exists)
            {
                context.Properties.Add(new Property 
                {
                    VocabularyId = sysVocab.Id,
                    LocalName = prop.LocalName,
                    Label = prop.Label,
                    TermUri = $"{SystemConstants.SystemNamespace}{prop.LocalName}"
                });
            }
        }

        await context.SaveChangesAsync();
        logger.LogInformation("System Properties seeded successfully.");
    }
}
