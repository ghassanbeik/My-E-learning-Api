

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
    {
        public void Configure(EntityTypeBuilder<Certificate> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.CertificateNumber).HasMaxLength(100).IsRequired();
            builder.Property(c => c.VerificationUrl).HasMaxLength(500);
            builder.HasIndex(c => c.CertificateNumber).IsUnique();
            builder.HasOne(c => c.Course).WithMany().HasForeignKey(c => c.CourseId).OnDelete(DeleteBehavior.Restrict);
        }
    }

}
