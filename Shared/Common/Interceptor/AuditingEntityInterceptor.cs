using DreamWeddingApi.Shared.Common.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DreamWeddingApi.Shared.Common.Interceptor;

public class AuditingEntityInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context is null)
        {
            return base.SavingChanges(eventData, result);
        }

        var entries = eventData.Context.ChangeTracker.Entries<AuditingEntity>().ToList();

        SetEntityUpdated(entries);
        SetEntityDeleted(entries);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var entries = eventData.Context.ChangeTracker.Entries<AuditingEntity>().ToList();

        SetEntityUpdated(entries);
        SetEntityDeleted(entries);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void SetEntityUpdated(IList<EntityEntry<AuditingEntity>> entries)
    {
        var updatedEntries = entries.Where(e => e.State == EntityState.Modified);

        foreach (var entry in updatedEntries)
        {
            entry.Entity.UpdatedAt = DateTime.Now;
        }
    }

    private static void SetEntityDeleted(IList<EntityEntry<AuditingEntity>> entries)
    {
        var deletedEntries = entries.Where(e => e.State == EntityState.Deleted);

        foreach (var entry in deletedEntries)
        {
            entry.State = EntityState.Modified;
            entry.Entity.Deleted = true;
            entry.Entity.DeletedAt = DateTime.Now;
        }
    }
}