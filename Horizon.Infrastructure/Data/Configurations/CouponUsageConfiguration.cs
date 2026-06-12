

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class CouponUsageConfiguration : IEntityTypeConfiguration<CouponUsage>
    {
        public void Configure(EntityTypeBuilder<CouponUsage> builder)
        {
            builder.HasKey(cu => cu.Id);
            builder.Property(cu => cu.DiscountAmount).HasPrecision(18, 2);
            builder.HasOne(cu => cu.User).WithMany().HasForeignKey(cu => cu.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(cu => cu.Enrollment).WithMany().HasForeignKey(cu => cu.EnrollmentId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
