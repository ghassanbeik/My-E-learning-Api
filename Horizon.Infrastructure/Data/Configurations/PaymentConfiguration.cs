

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.TransactionId).HasMaxLength(100).IsRequired();
            builder.Property(p => p.PaymentMethod).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Amount).HasPrecision(18, 2);
            builder.Property(p => p.Currency).HasMaxLength(10);
            builder.Property(p => p.RefundAmount).HasPrecision(18, 2);
            builder.Property(p => p.TaxAmount).HasPrecision(18, 2);
            builder.Property(p => p.PlatformFee).HasPrecision(18, 2);
            builder.Property(p => p.InstructorEarnings).HasPrecision(18, 2);
            builder.HasIndex(p => p.TransactionId).IsUnique();
            builder.HasIndex(p => p.Status);
            builder.HasMany(p => p.RefundRequests).WithOne(r => r.Payment).HasForeignKey(r => r.PaymentId).OnDelete(DeleteBehavior.Restrict);
        }
    }

}
