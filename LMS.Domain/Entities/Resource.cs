namespace LMS.Domain.Entities;

public abstract class Resource
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public string? OwnerId { get; set; } 

    public virtual ICollection<Value> Values { get; set; } = new List<Value>();
}