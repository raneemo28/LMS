namespace LMS.Domain.Entities;

public class ResourceTemplate
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string? Description { get; set; }
}