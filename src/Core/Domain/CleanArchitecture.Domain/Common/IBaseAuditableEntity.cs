namespace CleanArchitecture.Domain.Common;

public interface IBaseAuditableEntity
{
    DateTime CreatedOn { get; set; }
    string? CreatedBy { get; set; }
    DateTime? LastModifiedOn { get; set; }
    string? LastModifiedBy { get; set; }
    bool IsDeleted { get; set; }
    DateTime? DeletedOn { get; set; }
    string? DeletedBy { get; set; }
} 