namespace LMS.Domain.Entities;

public class Vocabulary
{
    public int Id { get; set; }
    public string Prefix { get; set; } = string.Empty;
    public string NamespaceUri { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;

 }