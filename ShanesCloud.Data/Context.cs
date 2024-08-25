using Microsoft.EntityFrameworkCore;
using ShanesCloud.Data.Entities;

namespace ShanesCloud.Data;

public class Context: DbContext
{
    #region Properties

    public DbSet<RoleClaim> RoleClaims { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<UserClaim> UserClaims { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<User> Users { get; set; }

    #endregion

    #region Construction

    public Context(DbContextOptions options): base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply All Configurations in ShanesCloud.Data
        builder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);
    }

    #endregion
}
