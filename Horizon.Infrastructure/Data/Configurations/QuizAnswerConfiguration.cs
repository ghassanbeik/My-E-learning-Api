
using Horizon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Horizon.Infrastructure.Data.Configurations
{
    public class QuizAnswerConfiguration : IEntityTypeConfiguration<QuizAnswer>
    {
        public void Configure(EntityTypeBuilder<QuizAnswer> builder)
        {
            builder.HasKey(qa => qa.Id);
            builder.HasOne(qa => qa.Question).WithMany().HasForeignKey(qa => qa.QuestionId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(qa => qa.SelectedAnswer).WithMany().HasForeignKey(qa => qa.SelectedAnswerId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
