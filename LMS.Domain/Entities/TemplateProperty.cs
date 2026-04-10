namespace LMS.Domain.Entities;

public class TemplateProperty
{
    public int TemplateId { get; set; }
    public int PropertyId { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
    public string? AlternateLabel { get; set; }

    public virtual ResourceTemplate Template { get; set; } = null!;
    public virtual Property Property { get; set; } = null!;
}