using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShanesCloud.Data.Entities.Configurations;

public class RoleConfiguration: IEntityTypeConfiguration<Role>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
               .HasConversion(id => id.Value, value => new RoleId(value))
               .ValueGeneratedOnAdd();

        builder.Property(r => r.ConcurrencyStamp)
               .IsConcurrencyToken();

        builder.Property(r => r.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(r => r.NormalizedName)
               .HasMaxLength(100)
               .IsRequired();

        builder.HasIndex(r => r.NormalizedName)
               .IsUnique();
    }

    #endregion
}
