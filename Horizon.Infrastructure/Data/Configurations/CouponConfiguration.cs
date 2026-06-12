

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Code).HasMaxLength(50).IsRequired();
            builder.Property(c => c.Value).HasPrecision(18, 2);
            builder.Property(c => c.MaxDiscountAmount).HasPrecision(18, 2);
            builder.Property(c => c.MinOrderAmount).HasPrecision(18, 2);
            builder.HasIndex(c => c.Code).IsUnique();
            builder.HasMany(c => c.Usages).WithOne(cu => cu.Coupon).HasForeignKey(cu => cu.CouponId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.ApplicableCourses).WithOne(cc => cc.Coupon).HasForeignKey(cc => cc.CouponId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.ApplicableCategories).WithOne(cc => cc.Coupon).HasForeignKey(cc => cc.CouponId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
