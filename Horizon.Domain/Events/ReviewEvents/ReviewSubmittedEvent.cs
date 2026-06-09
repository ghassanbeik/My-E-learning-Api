

namespace Horizon.Domain.Events.ReviewEvents
{
    public class ReviewSubmittedEvent : DomainEvent
    {
        public Guid ReviewId { get; init; }
        public Guid StudentId { get; init; }
        public Guid CourseId { get; init; }
        public Guid InstructorId { get; init; }
        public string StudentName { get; init; } = string.Empty;
        public string CourseTitle { get; init; } = string.Empty;
        public int Rating { get; init; }
    }
}
