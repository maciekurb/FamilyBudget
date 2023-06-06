using FamilyBudget.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FamilyBudget.Infrastructure.Data;

public class CreatedAtSaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result, 
        CancellationToken cancellationToken = default)
    {
        SetCreatedAt(eventData);
            
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, 
        InterceptionResult<int> result)
    {
        SetCreatedAt(eventData);
            
        return base.SavingChanges(eventData, result);
    }

    private static void SetCreatedAt(DbContextEventData eventData)
    {
        if (eventData.Context == null) 
            return;
        
        var entries = eventData.Context.ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            if (entry.Entity is IAuditableEntity auditableEntity && entry.State == EntityState.Added)
            {
                auditableEntity.CreatedAt = DateTime.UtcNow;
            }
        }
    }
}