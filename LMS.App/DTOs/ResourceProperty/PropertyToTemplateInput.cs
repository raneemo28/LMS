public record PropertyToTemplateInput(
    int PropertyId,
    bool IsRequired,
    int DisplayOrder,
    string? AlternateLabel
);