

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class BundleCourseConfiguration : IEntityTypeConfiguration<BundleCourse>
    {
        public void Configure(EntityTypeBuilder<BundleCourse> builder)
        {
            builder.HasKey(bc => bc.Id);
            builder.HasIndex(bc => new { bc.BundleId, bc.CourseId }).IsUnique();
        }
    }

}
