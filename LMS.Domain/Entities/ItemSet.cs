namespace LMS.Domain.Entities;

public class ItemSet : Resource
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
}