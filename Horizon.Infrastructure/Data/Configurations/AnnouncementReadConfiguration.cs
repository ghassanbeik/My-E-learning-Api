
using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class AnnouncementReadConfiguration : IEntityTypeConfiguration<AnnouncementRead>
    {
        public void Configure(EntityTypeBuilder<AnnouncementRead> builder)
        {
            builder.HasKey(ar => new { ar.AnnouncementId, ar.UserId });
            builder.HasOne(ar => ar.Announcement).WithMany(a => a.ReadBy).HasForeignKey(ar => ar.AnnouncementId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ar => ar.User).WithMany().HasForeignKey(ar => ar.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }

}
