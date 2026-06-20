using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class VerificationTokenConfiguration : IEntityTypeConfiguration<VerificationToken>
    {
        public void Configure(EntityTypeBuilder<VerificationToken> b)
        {
            b.HasKey(t => t.Id);

            b.Property(t => t.TokenHash)
             .HasMaxLength(128)
             .IsRequired();

            // Fast lookup by (hash, type) — the only query done at validation time.
            b.HasIndex(t => new { t.TokenHash, t.Type });

            // Supports "show pending tokens" and cleanup queries.
            b.HasIndex(t => new { t.UserId, t.Type, t.UsedAt, t.ExpiresAt });

            // IsValid is a computed C# property — not a DB column.
            b.Ignore(t => t.IsValid);

            b.HasOne(t => t.User)
             .WithMany()
             .HasForeignKey(t => t.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
