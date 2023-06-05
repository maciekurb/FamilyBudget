namespace FamilyBudget.Domain.Common;

public abstract class BaseEntity : IEntity, IAuditableEntity, ISoftDelete
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public bool IsDeleted { get; protected set; }

    public virtual void Delete()
    {
        IsDeleted = false;
    }
}