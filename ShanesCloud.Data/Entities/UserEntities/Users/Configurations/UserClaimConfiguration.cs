using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShanesCloud.Data.Entities.Configurations;

public class UserClaimConfiguration: IEntityTypeConfiguration<UserClaim>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.ToTable("UserClaims");

        builder.HasKey(uc => uc.Id);

        builder.Property(uc => uc.Id)
               .HasConversion(id => id.Value, value => new UserClaimId(value))
               .ValueGeneratedOnAdd();

        builder.Property(uc => uc.ClaimType)
               .HasMaxLength(500);

        builder.Property(uc => uc.ClaimValue)
               .HasMaxLength(500);

        // Each user can have many UserClaims
        builder.HasOne(uc => uc.User)
               .WithMany(u => u.UserClaims)
               .HasForeignKey(uc => uc.UserId)
               .IsRequired();
    }

    #endregion
}
