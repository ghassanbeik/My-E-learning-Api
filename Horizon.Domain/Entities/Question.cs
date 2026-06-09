
namespace Horizon.Domain.Entities
{
    public class Question : AuditableEntity
    {
        public Guid QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
        public string Text { get; set; } = string.Empty;
        public string? Explanation { get; set; }
        public int Points { get; set; } = 1;
        public int DisplayOrder { get; set; } = 0;
        public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
    }
}
