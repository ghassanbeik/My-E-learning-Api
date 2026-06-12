
using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class BundleConfiguration : IEntityTypeConfiguration<Bundle>
    {
        public void Configure(EntityTypeBuilder<Bundle> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Title).HasMaxLength(200).IsRequired();
            builder.Property(b => b.Price).HasPrecision(18, 2);
            builder.Property(b => b.DiscountPrice).HasPrecision(18, 2);
            builder.HasMany(b => b.BundleCourses).WithOne(bc => bc.Bundle).HasForeignKey(bc => bc.BundleId).OnDelete(DeleteBehavior.Cascade);
        }
    }


}
