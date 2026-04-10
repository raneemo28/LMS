namespace LMS.Domain.Entities;

public class Media : Resource
{
    public int? ItemId { get; set; }
    public string StoragePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string? MimeType { get; set; }
    public long? FileSize { get; set; }
    public string? AltText { get; set; }

    public virtual Item? Item { get; set; }
}