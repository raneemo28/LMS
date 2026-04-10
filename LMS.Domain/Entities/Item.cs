namespace LMS.Domain.Entities;

public class Item : Resource
{
    public int? TemplateId { get; set; }
    public virtual ResourceTemplate? Template { get; set; }
}