

namespace Horizon.Domain.Events.EnrollmentEvents
{
    public class LessonCompletedEvent : DomainEvent
    {
        public Guid EnrollmentId { get; init; }
        public Guid StudentId { get; init; }
        public Guid LessonId { get; init; }
        public Guid CourseId { get; init; }
        public double ProgressPercentage { get; init; }
    }
}
