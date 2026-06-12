
using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class PlatformAnalyticsConfiguration : IEntityTypeConfiguration<PlatformAnalytics>
    {
        public void Configure(EntityTypeBuilder<PlatformAnalytics> builder)
        {
            builder.HasKey(pa => pa.Id);
            builder.HasIndex(pa => pa.Date).IsUnique();
            builder.Property(pa => pa.TotalRevenue).HasPrecision(18, 2);
        }
    }

}
