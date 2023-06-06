using FamilyBudget.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FamilyBudget.Infrastructure.Data;

public class ModifiedAtSaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken cancellationToken = default)
    {
        SetModifiedAt(eventData);
            
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, 
        InterceptionResult<int> result)
    {
        SetModifiedAt(eventData);
            
        return base.SavingChanges(eventData, result);
    }

    private static void SetModifiedAt(DbContextEventData eventData)
    {
        if (eventData.Context == null) 
            return;
        
        var entries = eventData.Context.ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            if (entry.Entity is IAuditableEntity auditableEntity && entry.State == EntityState.Modified)
            {
                auditableEntity.ModifiedAt = DateTime.UtcNow;
            }
        }
    }
}

