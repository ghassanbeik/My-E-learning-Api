

namespace Horizon.Domain.Events.CertificateEvents
{
    public class CertificateIssuedEvent : DomainEvent
    {
        public Guid CertificateId { get; init; }
        public Guid StudentId { get; init; }
        public Guid CourseId { get; init; }
        public string StudentEmail { get; init; } = string.Empty;
        public string StudentName { get; init; } = string.Empty;
        public string CourseTitle { get; init; } = string.Empty;
        public string CertificateNumber { get; init; } = string.Empty;
        public string? VerificationUrl { get; init; }
    }
}
