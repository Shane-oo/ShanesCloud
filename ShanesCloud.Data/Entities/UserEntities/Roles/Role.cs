using ShanesCloud.Data.Entities.Core;

namespace ShanesCloud.Data.Entities;

public class Role: Entity<RoleId>, IAuditableEntity
{
    #region Properties

    public string Name { get; set; }

    public string NormalizedName { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public Guid ConcurrencyStamp { get; set; } = Guid.NewGuid();

    public virtual ICollection<RoleClaim> RoleClaims { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }

    public Roles RoleEnum => Name switch
                             {
                                 nameof(Roles.Admin) => Roles.Admin,
                                 nameof(Roles.User) => Roles.User,
                                 _ => Roles.Guest
                             };
    #endregion
}
