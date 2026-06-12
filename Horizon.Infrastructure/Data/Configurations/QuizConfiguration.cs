

using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasKey(q => q.Id);
            builder.Property(q => q.Title).HasMaxLength(200).IsRequired();
            builder.HasMany(q => q.Questions).WithOne(q => q.Quiz).HasForeignKey(q => q.QuizId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(q => q.Attempts).WithOne(a => a.Quiz).HasForeignKey(a => a.QuizId).OnDelete(DeleteBehavior.Restrict);
        }
    }

}
