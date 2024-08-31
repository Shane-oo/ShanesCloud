using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShanesCloud.Data.Entities.Configurations;

public class ApplicationConfiguration: IEntityTypeConfiguration<Application>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.ToTable("Applications");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.ClientId)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(a => a.ClientSecret)
               .HasMaxLength(500);

        builder.Property(a => a.ConcurrencyToken)
               .HasMaxLength(36)
               .IsConcurrencyToken();

        builder.Property(a => a.ConsentType)
               .HasMaxLength(50);

        builder.Property(a => a.DisplayName)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(a => a.DisplayNames)
               .HasMaxLength(1000);

        builder.Property(a => a.Permissions)
               .IsRequired()
               .HasMaxLength(1000);

        builder.Property(a => a.PostLogoutRedirectUris)
               .HasMaxLength(-1);

        builder.Property(a => a.Properties)
               .HasMaxLength(2000);

        builder.Property(a => a.RedirectUris)
               .HasMaxLength(-1);

        builder.Property(a => a.Requirements)
               .HasMaxLength(100);

        builder.Property(a => a.ClientType)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(a => a.ApplicationType)
               .HasMaxLength(500);


        builder.Property(a => a.JsonWebKeySet)
               .HasMaxLength(2000);

        builder.Property(a => a.Settings)
               .HasMaxLength(1000);

        builder.HasIndex(a => a.ClientId)
               .IsUnique();
    }

    #endregion
}
