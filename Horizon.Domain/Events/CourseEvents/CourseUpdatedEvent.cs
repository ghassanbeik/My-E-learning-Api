
namespace Horizon.Domain.Events.CourseEvents
{
    public class CourseUpdatedEvent : DomainEvent
    {
        public Guid CourseId { get; init; }
        public Guid InstructorId { get; init; }
        public string CourseTitle { get; init; } = string.Empty;
    }
}
