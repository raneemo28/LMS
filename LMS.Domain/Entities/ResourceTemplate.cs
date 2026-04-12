namespace LMS.Domain.Entities;

public class ResourceTemplate
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string? Description { get; set; }

    public virtual ICollection<TemplateProperty> Properties { get; set; } = new List<TemplateProperty>();
    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}