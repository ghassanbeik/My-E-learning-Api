

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class SearchLogConfiguration : IEntityTypeConfiguration<SearchLog>
    {
        public void Configure(EntityTypeBuilder<SearchLog> builder)
        {
            builder.HasKey(sl => sl.Id);
            builder.Property(sl => sl.Query).HasMaxLength(500).IsRequired();
            builder.HasIndex(sl => sl.SearchedAt);
            builder.HasOne(sl => sl.User).WithMany().HasForeignKey(sl => sl.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
