using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShanesCloud.Data.Entities.Configurations;

public class AuthorizationConfiguration: IEntityTypeConfiguration<Authorization>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Authorization> builder)
    {
        builder.ToTable("Authorizations");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.ConcurrencyToken)
               .HasMaxLength(36)
               .IsConcurrencyToken();

        builder.Property(a => a.Properties)
               .HasMaxLength(2000);

        builder.Property(a => a.Scopes)
               .HasMaxLength(500);

        builder.Property(a => a.Status)
               .HasMaxLength(50);

        builder.Property(a => a.Subject)
               .IsRequired()
               .HasMaxLength(36);

        builder.Property(a => a.Type)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasOne(a => a.Application)
               .WithMany(application => application.Authorizations)
               .HasForeignKey(a => a.ApplicationId)
               .IsRequired(false);

        builder.HasIndex(a => new { a.ApplicationId, a.Status, a.Subject, a.Type });
    }

    #endregion
}
