
using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class LiveSessionConfiguration : IEntityTypeConfiguration<LiveSession>
    {
        public void Configure(EntityTypeBuilder<LiveSession> builder)
        {
            builder.HasKey(ls => ls.Id);
            builder.Property(ls => ls.Title).HasMaxLength(200).IsRequired();
            builder.Property(ls => ls.MeetingUrl).HasMaxLength(500);
            builder.Property(ls => ls.RecordingUrl).HasMaxLength(500);
            builder.HasIndex(ls => ls.ScheduledAt);
            builder.HasOne(ls => ls.Instructor).WithMany().HasForeignKey(ls => ls.InstructorId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
