using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShanesCloud.Data.Entities.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .HasConversion(id => id.Value, value => new UserId(value))
               .ValueGeneratedOnAdd();

        builder.Property(u => u.UserName)
               .HasMaxLength(30)
               .IsRequired();

        builder.Property(u => u.NormalizedUserName)
               .HasMaxLength(30)
               .IsRequired();

        builder.Property(u => u.Email)
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(u => u.NormalizedEmail)
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(u => u.Password)
               .HasMaxLength(1024);

        builder.Property(u => u.SecurityStamp)
               .HasMaxLength(1024);

        builder.Property(u => u.ConcurrencyStamp)
               .IsConcurrencyToken();

        builder.HasIndex(u => u.NormalizedUserName)
               .IsUnique();
    }

    #endregion
}
