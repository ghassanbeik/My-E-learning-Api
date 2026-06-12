
using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
    {
        public void Configure(EntityTypeBuilder<Announcement> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Title).HasMaxLength(200).IsRequired();
            builder.Property(a => a.Content).HasMaxLength(5000).IsRequired();
            builder.HasOne(a => a.Instructor).WithMany().HasForeignKey(a => a.InstructorId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(a => a.ReadBy).WithOne(ar => ar.Announcement).HasForeignKey(ar => ar.AnnouncementId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
