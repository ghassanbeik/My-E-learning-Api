

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Comment).HasMaxLength(2000);
            builder.HasIndex(r => new { r.StudentId, r.CourseId }).IsUnique();
            builder.HasIndex(r => r.Status);
            builder.HasMany(r => r.Responses).WithOne(rr => rr.Review).HasForeignKey(rr => rr.ReviewId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(r => r.Votes).WithOne(rv => rv.Review).HasForeignKey(rv => rv.ReviewId).OnDelete(DeleteBehavior.Cascade);
        }
    }

}
