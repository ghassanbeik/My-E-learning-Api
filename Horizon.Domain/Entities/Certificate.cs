

namespace Horizon.Domain.Entities
{
    public class Certificate : AuditableEntity
    {
        public Guid EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public Guid StudentId { get; set; }
        public UserInfo Student { get; set; } = null!;
        public string CertificateNumber { get; set; } = string.Empty;
        public string? TemplateUrl { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiryDate { get; set; }
        public string? VerificationUrl { get; set; }
        public bool IsRevoked { get; set; } = false;
        public string? RevokeReason { get; set; }
    }
}
