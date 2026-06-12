
using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.AmountPaid).HasPrecision(18, 2);
            builder.Property(e => e.DiscountApplied).HasPrecision(18, 2);
            builder.Property(e => e.ProgressPercentage).HasPrecision(5, 2);
            builder.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique();
            builder.HasIndex(e => e.Status);
            builder.HasMany(e => e.Progresses).WithOne(p => p.Enrollment).HasForeignKey(p => p.EnrollmentId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(e => e.Certificates).WithOne(c => c.Enrollment).HasForeignKey(c => c.EnrollmentId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(e => e.Payments).WithOne(p => p.Enrollment).HasForeignKey(p => p.EnrollmentId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
