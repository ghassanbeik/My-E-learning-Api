
namespace Horizon.Domain.Entities
{
    public class QuizAttempt : AuditableEntity
    {
        public Guid QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
        public Guid StudentId { get; set; }
        public UserInfo Student { get; set; } = null!;
        public int Score { get; set; } = 0;
        public int MaxScore { get; set; } = 0;
        public bool IsPassed { get; set; } = false;
        public int AttemptNumber { get; set; } = 1;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public TimeSpan? TimeTaken { get; set; }
        public ICollection<QuizAnswer> Answers { get; set; } = new List<QuizAnswer>();
    }
}
