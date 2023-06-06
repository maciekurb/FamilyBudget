namespace FamilyBudget.Domain.Common;

public interface ISoftDelete
{
    bool IsDeleted { get; }
    void Delete();
}