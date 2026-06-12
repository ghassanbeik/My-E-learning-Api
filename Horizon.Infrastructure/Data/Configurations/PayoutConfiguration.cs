

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class PayoutConfiguration : IEntityTypeConfiguration<Payout>
    {
        public void Configure(EntityTypeBuilder<Payout> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Amount).HasPrecision(18, 2);
            builder.Property(p => p.TotalRevenue).HasPrecision(18, 2);
            builder.Property(p => p.PlatformFee).HasPrecision(18, 2);
            builder.HasIndex(p => p.Status);
            builder.HasOne(p => p.Instructor).WithMany().HasForeignKey(p => p.InstructorId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
