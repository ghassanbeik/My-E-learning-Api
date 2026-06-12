

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class InstructorSubscriberConfiguration : IEntityTypeConfiguration<InstructorSubscriber>
    {
        public void Configure(EntityTypeBuilder<InstructorSubscriber> builder)
        {
            builder.HasKey(s => new { s.InstructorId, s.SubscriberId });
            builder.HasOne(s => s.Instructor).WithMany(u => u.Subscribers).HasForeignKey(s => s.InstructorId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.Subscriber).WithMany(u => u.Subscriptions).HasForeignKey(s => s.SubscriberId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
