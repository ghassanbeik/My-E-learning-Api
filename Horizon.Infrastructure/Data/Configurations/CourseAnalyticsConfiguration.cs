

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class CourseAnalyticsConfiguration : IEntityTypeConfiguration<CourseAnalytics>
    {
        public void Configure(EntityTypeBuilder<CourseAnalytics> builder)
        {
            builder.HasKey(ca => ca.Id);
            builder.HasIndex(ca => new { ca.CourseId, ca.Date }).IsUnique();
            builder.Property(ca => ca.Revenue).HasPrecision(18, 2);
            builder.Property(ca => ca.Refunds).HasPrecision(18, 2);
            builder.HasOne(ca => ca.Course).WithMany().HasForeignKey(ca => ca.CourseId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
