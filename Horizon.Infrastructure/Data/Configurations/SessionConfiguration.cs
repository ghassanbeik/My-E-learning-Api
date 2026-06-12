

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.RefreshToken).HasMaxLength(500).IsRequired();
            builder.HasIndex(s => s.RefreshToken).IsUnique();
            builder.HasIndex(s => new { s.UserId, s.RevokedAt });
            builder.Ignore(s => s.IsActive);
            builder.HasOne(s => s.User).WithMany().HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
