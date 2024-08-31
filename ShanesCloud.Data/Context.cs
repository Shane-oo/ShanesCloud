using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ShanesCloud.Data.Entities;
using ShanesCloud.Data.Entities.Core;

namespace ShanesCloud.Data;

public class Context: DbContext
{
    #region Properties

    public DbSet<Application> Applications { get; set; }

    public DbSet<Authorization> Authorizations { get; set; }

    public DbSet<RoleClaim> RoleClaims { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Scope> Scopes { get; set; }

    public DbSet<Token> Tokens { get; set; }

    public DbSet<UserClaim> UserClaims { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<User> Users { get; set; }

    #endregion

    #region Construction

    public Context(DbContextOptions options): base(options)
    {
    }

    #endregion

    #region Private Methods

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Apply All Configurations in ShanesCloud.Data
        builder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);

        ApplySoftDeleteQueryFilter(builder);
    }

    private static void ApplySoftDeleteQueryFilter(ModelBuilder modelBuilder)
    {
        foreach(var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            var param = Expression.Parameter(entityType.ClrType, "entity");
            var prop = Expression.PropertyOrField(param, nameof(ISoftDeletable.IsDeleted));
            var entityNotDeleted = Expression.Lambda(Expression.Equal(prop, Expression.Constant(false)), param);

            entityType.SetQueryFilter(entityNotDeleted);
        }
    }

    #endregion
}
