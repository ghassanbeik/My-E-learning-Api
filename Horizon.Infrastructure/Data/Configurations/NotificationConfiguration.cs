

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Title).HasMaxLength(200).IsRequired();
            builder.Property(n => n.Message).HasMaxLength(2000).IsRequired();
            builder.Property(n => n.ActionUrl).HasMaxLength(500);
            builder.Property(n => n.ImageUrl).HasMaxLength(500);
            builder.Property(n => n.SenderName).HasMaxLength(100);
            builder.Property(n => n.RelatedEntityType).HasMaxLength(50);
            builder.HasIndex(n => new { n.RecipientId, n.Status });
            builder.HasIndex(n => n.CreatedAt);
        }
    }

}
