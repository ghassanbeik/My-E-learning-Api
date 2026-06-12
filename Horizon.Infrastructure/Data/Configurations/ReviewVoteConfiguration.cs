

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class ReviewVoteConfiguration : IEntityTypeConfiguration<ReviewVote>
    {
        public void Configure(EntityTypeBuilder<ReviewVote> builder)
        {
            builder.HasKey(rv => rv.Id);
            builder.HasIndex(rv => new { rv.ReviewId, rv.UserId }).IsUnique();
            builder.HasOne(rv => rv.User).WithMany().HasForeignKey(rv => rv.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }

}
