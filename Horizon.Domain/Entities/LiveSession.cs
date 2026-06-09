

namespace Horizon.Domain.Entities
{
    public class LiveSession : AuditableEntity
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public Guid InstructorId { get; set; }
        public UserInfo Instructor { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime ScheduledAt { get; set; }
        public int DurationMinutes { get; set; } = 60;
        public string? MeetingUrl { get; set; }
        public string? RecordingUrl { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int AttendeeCount { get; set; } = 0;
    }
}
