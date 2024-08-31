using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShanesCloud.Data.Entities.Configurations;

public class TokenConfiguration: IEntityTypeConfiguration<Token>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.ToTable("Tokens");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.ConcurrencyToken)
               .HasMaxLength(36)
               .IsConcurrencyToken();

        builder.Property(t => t.ReferenceId)
               .HasMaxLength(100);

        builder.Property(t => t.Status)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(t => t.Subject)
               .HasMaxLength(36);

        builder.Property(t => t.Type)
               .HasMaxLength(50);

        builder.Property(t => t.Payload)
               .HasMaxLength(10000);

        builder.Property(t => t.Properties)
               .HasMaxLength(400);

        builder.HasOne(t => t.Authorization)
               .WithMany(a => a.Tokens)
               .HasForeignKey(t => t.AuthorizationId)
               .IsRequired(false);

        builder.HasOne(t => t.Application)
               .WithMany(a => a.Tokens)
               .HasForeignKey(t => t.ApplicationId)
               .IsRequired(false);

        builder.HasIndex(t => t.ReferenceId)
               .IsUnique();

        builder.HasIndex(t => new { t.ApplicationId, t.Status, t.Subject, t.Type });
    }

    #endregion
}
