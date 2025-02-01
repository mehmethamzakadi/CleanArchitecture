namespace CleanArchitecture.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity, IBaseAuditableEntity
{
    public new DateTime CreatedOn { get; set; }
    public new string? CreatedBy { get; set; }
    public new DateTime? LastModifiedOn { get; set; }
    public new string? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOn { get; set; }
    public string? DeletedBy { get; set; }

    public void MarkAsDeleted(string deletedBy)
    {
        IsDeleted = true;
        DeletedOn = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
} 