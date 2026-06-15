namespace TaskFlowPro.Domain.Common.Models;

public abstract class AuditableEntity : Entity
{
    public DateTimeOffset CreatedAt { get; set; }
    
    // Storing user ID as a string to accommodate potential future OAuth providers
    public string? CreatedBy { get; set; }
    
    public DateTimeOffset? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
}