

namespace Horizon.Domain.Events.EnrollmentEvents
{
    public class StudentEnrolledEvent : DomainEvent
    {
        public Guid EnrollmentId { get; init; }
        public Guid StudentId { get; init; }
        public Guid CourseId { get; init; }
        public Guid InstructorId { get; init; }
        public string StudentEmail { get; init; } = string.Empty;
        public string StudentName { get; init; } = string.Empty;
        public string CourseTitle { get; init; } = string.Empty;
        public decimal AmountPaid { get; init; }
    }
}
