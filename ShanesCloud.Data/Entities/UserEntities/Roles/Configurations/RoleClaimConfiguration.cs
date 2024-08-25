using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShanesCloud.Data.Entities.Configurations;

public class RoleClaimConfiguration: IEntityTypeConfiguration<RoleClaim>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable("RoleClaims");

        builder.HasKey(rc => rc.Id);

        builder.Property(rc => rc.Id)
               .HasConversion(id => id.Value, value => new RoleClaimId(value))
               .ValueGeneratedOnAdd();

        builder.Property(rc => rc.ClaimType)
               .HasMaxLength(500);

        builder.Property(rc => rc.ClaimValue)
               .HasMaxLength(500);

        // Each Role can have many associated RoleClaims
        builder.HasOne(rc => rc.Role)
               .WithMany(r => r.RoleClaims)
               .HasForeignKey(rc => rc.RoleId)
               .IsRequired();
    }

    #endregion
}
