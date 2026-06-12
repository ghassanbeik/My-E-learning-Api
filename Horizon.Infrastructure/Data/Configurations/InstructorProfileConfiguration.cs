using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class InstructorProfileConfiguration : IEntityTypeConfiguration<InstructorProfile>
    {
        public void Configure(EntityTypeBuilder<InstructorProfile> builder)
        {
            builder.HasKey(ip => ip.Id);
            builder.Property(ip => ip.AverageRating).HasPrecision(5, 2);
            builder.Property(ip => ip.TotalEarnings).HasPrecision(18, 2);
            builder.Property(ip => ip.PendingEarnings).HasPrecision(18, 2);
            builder.HasOne(ip => ip.User).WithMany(u => u.InstructorProfiles).HasForeignKey(ip => ip.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
