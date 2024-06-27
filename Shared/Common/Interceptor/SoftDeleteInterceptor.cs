using DreamWeddingApi.Shared.Common.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DreamWeddingApi.Shared.Common.Interceptor;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var entries =
            eventData.Context.ChangeTracker
                .Entries<AuditingEntity>()
                .Where(e => e.State == EntityState.Deleted);

        foreach (var entry in entries)
        {
            entry.State = EntityState.Modified;
            entry.Entity.Deleted = true;
            entry.Entity.DeletedAt = DateTime.Now;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}