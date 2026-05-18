namespace LMS.App.DTOs.ResourceProperty;

public record ResourcePropertyDto
{
    public int PropertyId { get; init; }
    public string LocalName { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
    public string TermUri { get; init; } = string.Empty;
    public bool IsRequired { get; init; }
    public int DisplayOrder { get; init; }
    public string? AlternateLabel { get; init; }
}
