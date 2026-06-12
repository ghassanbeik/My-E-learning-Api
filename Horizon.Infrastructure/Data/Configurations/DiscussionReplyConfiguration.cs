

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class DiscussionReplyConfiguration : IEntityTypeConfiguration<DiscussionReply>
    {
        public void Configure(EntityTypeBuilder<DiscussionReply> builder)
        {
            builder.HasKey(dr => dr.Id);
            builder.Property(dr => dr.Content).HasMaxLength(5000).IsRequired();
            builder.HasOne(dr => dr.ParentReply).WithMany(dr => dr.ChildReplies).HasForeignKey(dr => dr.ParentReplyId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(dr => dr.Votes).WithOne(dv => dv.Reply).HasForeignKey(dv => dv.ReplyId).OnDelete(DeleteBehavior.Restrict);
        }
    }

}
