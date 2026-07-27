namespace BudgetManager.Domain.Entities;

public abstract class AuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    protected AuditableEntity()
    {
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public void Delete(string? deletedBy = null)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }

    public void Update(string? modifiedBy = null)
    {
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = modifiedBy;
    }
}
