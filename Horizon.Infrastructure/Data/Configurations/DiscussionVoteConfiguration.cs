

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class DiscussionVoteConfiguration : IEntityTypeConfiguration<DiscussionVote>
    {
        public void Configure(EntityTypeBuilder<DiscussionVote> builder)
        {
            builder.HasKey(dv => dv.Id);
            builder.HasOne(dv => dv.User).WithMany().HasForeignKey(dv => dv.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
