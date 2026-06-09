

namespace Horizon.Domain.Entities
{
    public class Progress : BaseEntity
    {
        public Guid EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedAt { get; set; }
        public int TimeSpentMinutes { get; set; } = 0;
        public int? VideoWatchedSeconds { get; set; }
        public int? VideoTotalSeconds { get; set; }
        public DateTime? LastAccessedAt { get; set; }
        public int AttemptCount { get; set; } = 0;
    }
}
