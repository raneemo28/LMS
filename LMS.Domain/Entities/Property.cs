namespace LMS.Domain.Entities;

public class Property
{
    public int Id { get; set; }
    public int VocabularyId { get; set; }
    public string LocalName { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string TermUri { get; set; } = string.Empty;

    public virtual Vocabulary Vocabulary { get; set; } = null!;
}