namespace LMS.Domain.Entities;

public class Value
{
    public int Id { get; set; }
    public int ResourceId { get; set; }
    public int PropertyId { get; set; }
    public string? ValueText { get; set; }
    public string? ValueUri { get; set; }
    public int? ValueResourceId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? Language { get; set; }

    public virtual Resource Resource { get; set; } = null!;
    public virtual Property Property { get; set; } = null!;
    public virtual Resource? ValueResource { get; set; }
}