

namespace Horizon.Domain.Entities
{
    public class Quiz : AuditableEntity
    {
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string? Instructions { get; set; }
        public int TimeLimitMinutes { get; set; } = 0;
        public int PassingScore { get; set; } = 70;
        public int MaxAttempts { get; set; } = 0;
        public bool ShuffleQuestions { get; set; } = false;
        public bool ShowCorrectAnswers { get; set; } = true;
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<QuizAttempt> Attempts { get; set; } = new List<QuizAttempt>();
    }
}
