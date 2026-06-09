

namespace Horizon.Domain.Entities
{
    public class QuizAnswer : BaseEntity
    {
        public Guid AttemptId { get; set; }
        public QuizAttempt Attempt { get; set; } = null!;
        public Guid QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public Guid SelectedAnswerId { get; set; }
        public AnswerOption SelectedAnswer { get; set; } = null!;
        public bool IsCorrect { get; set; } = false;
        public int PointsEarned { get; set; } = 0;
    }
}
