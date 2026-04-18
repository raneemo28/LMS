namespace LMS.App.DTOs.Vocabulary;

public record TemplatePropertyDto(
    boolean IsRequired,
    int DisplayOrder,
    string AlternativeLabel
);
 