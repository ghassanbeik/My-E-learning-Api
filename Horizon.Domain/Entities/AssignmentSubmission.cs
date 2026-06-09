

namespace Horizon.Domain.Entities
{
    public class AssignmentSubmission : AuditableEntity
    {
        public Guid AssignmentId { get; set; }
        public Assignment Assignment { get; set; } = null!;
        public Guid StudentId { get; set; }
        public UserInfo Student { get; set; } = null!;
        public string? SubmissionText { get; set; }
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public int? Score { get; set; }
        public string? Feedback { get; set; }
        public bool IsGraded { get; set; } = false;
        public DateTime? GradedAt { get; set; }
        public Guid? GradedById { get; set; }
        public UserInfo? GradedBy { get; set; }
        public int AttemptNumber { get; set; } = 1;
        public bool IsLate { get; set; } = false;
    }
}
