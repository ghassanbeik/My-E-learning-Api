

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(ci => ci.Id);
            builder.HasIndex(ci => new { ci.UserId, ci.CourseId }).IsUnique();
            builder.Property(ci => ci.DiscountAmount).HasPrecision(18, 2);
        }
    }
}
