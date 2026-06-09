

namespace Horizon.Domain.Events.EnrollmentEvents
{
    public class CourseCompletedEvent : DomainEvent
    {
        public Guid EnrollmentId { get; init; }
        public Guid StudentId { get; init; }
        public Guid CourseId { get; init; }
        public string StudentEmail { get; init; } = string.Empty;
        public string StudentName { get; init; } = string.Empty;
        public string CourseTitle { get; init; } = string.Empty;
    }
}
