

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class RefundRequestConfiguration : IEntityTypeConfiguration<RefundRequest>
    {
        public void Configure(EntityTypeBuilder<RefundRequest> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Reason).HasMaxLength(2000).IsRequired();
            builder.Property(r => r.AdminNote).HasMaxLength(2000);
            builder.HasIndex(r => r.Status);
            builder.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(r => r.ResolvedBy).WithMany().HasForeignKey(r => r.ResolvedById).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
