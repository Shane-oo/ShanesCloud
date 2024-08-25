using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShanesCloud.Data.Entities.Configurations;

public class UserRoleConfiguration: IEntityTypeConfiguration<UserRole>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(ur => new { ur.UserId, ur.RoleId });

        // Each User can only have one role
        builder.HasOne(ur => ur.User)
               .WithOne(u => u.UserRole)
               .HasForeignKey<UserRole>(ur => ur.UserId)
               .IsRequired();

        // Each Role can have many entries in the UserRole join table
        builder.HasOne(ur => ur.Role)
               .WithMany(r => r.UserRoles)
               .HasForeignKey(ur => ur.RoleId)
               .IsRequired();

        builder.HasIndex(ur => new { ur.UserId, ur.RoleId })
               .IsUnique();
    }

    #endregion
}
