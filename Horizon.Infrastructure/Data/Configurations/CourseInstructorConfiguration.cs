

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class CourseInstructorConfiguration : IEntityTypeConfiguration<CourseInstructor>
    {
        public void Configure(EntityTypeBuilder<CourseInstructor> builder)
        {
            builder.HasKey(ci => ci.Id);
            builder.HasIndex(ci => new { ci.CourseId, ci.InstructorId }).IsUnique();
            builder.Property(ci => ci.RevenueSharePercentage).HasPrecision(5, 2);
        }
    }
}
