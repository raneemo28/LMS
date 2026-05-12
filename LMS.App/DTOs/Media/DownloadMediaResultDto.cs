namespace LMS.App.DTOs.Media;

public class DownloadMediaResult
{
    public Stream Stream { get; set; } = default!;
    public string ContentType { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
}
