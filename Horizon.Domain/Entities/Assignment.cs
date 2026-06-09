

namespace Horizon.Domain.Entities
{
    public class Assignment : AuditableEntity
    {
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Instructions { get; set; }
        public int? MaxFileSizeMB { get; set; } = 50;
        public string[]? AllowedFileTypes { get; set; }
        public int? TimeLimitHours { get; set; }
        public int TotalPoints { get; set; } = 100;
        public DateTime? DueDate { get; set; }
        public bool AllowLateSubmission { get; set; } = false;
        public int LatePenaltyPercentage { get; set; } = 0;
        public ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
    }
}
