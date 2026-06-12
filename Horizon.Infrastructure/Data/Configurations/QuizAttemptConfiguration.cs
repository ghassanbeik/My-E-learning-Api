
using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
    {
        public void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            builder.HasKey(qa => qa.Id);
            builder.HasOne(qa => qa.Student).WithMany().HasForeignKey(qa => qa.StudentId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(qa => qa.Answers).WithOne(a => a.Attempt).HasForeignKey(a => a.AttemptId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
