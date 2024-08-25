using ShanesCloud.Data.Entities.Core;

namespace ShanesCloud.Data.Entities;

public class User: Entity<UserId>, IAuditableEntity, ISoftDeletable
{
    #region Properties

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public string UserName { get; set; }

    public string NormalizedUserName { get; set; }

    public string Email { get; set; }

    public string NormalizedEmail { get; set; }

    public string Password { get; set; }

    public DateTimeOffset? LoginOn { get; set; }

    public Guid ConcurrencyStamp { get; set; } = Guid.NewGuid();

    public bool LockoutEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public string SecurityStamp { get; set; }

    public int AccessFailedCount { get; set; }

    public bool EmailConfirmed { get; set; }

    public ICollection<UserClaim> UserClaims { get; set; }

    public UserRole UserRole { get; set; }

    #endregion

    #region Public Methods

    public void AddRole(RoleId roleId)
    {
        UserRole = new UserRole(roleId, Id);
    }

    #endregion
}
