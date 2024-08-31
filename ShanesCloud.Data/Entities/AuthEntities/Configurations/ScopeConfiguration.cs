using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShanesCloud.Data.Entities.Configurations;

public class ScopeConfiguration: IEntityTypeConfiguration<Scope>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Scope> builder)
    {
        builder.ToTable("Scopes");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.ConcurrencyToken)
               .HasMaxLength(36)
               .IsConcurrencyToken();

        builder.Property(s => s.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(s => s.DisplayName)
               .HasMaxLength(200);

        builder.Property(s => s.DisplayNames)
               .HasMaxLength(1000);

        builder.Property(s => s.Description)
               .HasMaxLength(500);

        builder.Property(s => s.Descriptions)
               .HasMaxLength(5000);

        builder.Property(a => a.Properties)
               .HasMaxLength(5000);

        builder.Property(a => a.Resources)
               .HasMaxLength(1000);

        builder.HasIndex(s => s.Name)
               .IsUnique();
    }

    #endregion
}
