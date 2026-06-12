

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class ReviewResponseConfiguration : IEntityTypeConfiguration<ReviewResponse>
    {
        public void Configure(EntityTypeBuilder<ReviewResponse> builder)
        {
            builder.HasKey(rr => rr.Id);
            builder.HasOne(rr => rr.Responder).WithMany().HasForeignKey(rr => rr.ResponderId).OnDelete(DeleteBehavior.Restrict);
        }
    }

}
