public record CreateResourceValueDto(
    int PropertyId,
    string Valuetext?,
    string ValueUri?,
    string ValueType,
    string ValueLanguage?
);