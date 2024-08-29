using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ShanesCloud.Data.Entities.Core;

public class EntityInterceptor: SaveChangesInterceptor
{
    #region Public Methods

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
                                                                          InterceptionResult<int> result,
                                                                          CancellationToken cancellationToken = new CancellationToken())
    {
        var context = eventData.Context;

        if (context == null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var auditableEntries = context.ChangeTracker.Entries<IAuditableEntity>();

        foreach(var entityEntry in auditableEntries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(a => a.CreatedOn).CurrentValue = DateTimeOffset.Now;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(a => a.ModifiedOn).CurrentValue = DateTimeOffset.Now;
            }
        }

        var deletedEntries = context.ChangeTracker.Entries<ISoftDeletable>();
        foreach(var entityEntry in deletedEntries)
        {
            if (entityEntry.State != EntityState.Deleted) continue;

            entityEntry.Property(s => s.IsDeleted).CurrentValue = true;

            entityEntry.Property(a => a.DeletedOn).CurrentValue = DateTimeOffset.Now;

            // don't delete
            entityEntry.State = EntityState.Modified;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    #endregion
}
