
using Horizon.Domain.Enums;
using System;

namespace Horizon.Domain.Entities
{
    public class Enrollment : AuditableEntity
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public Guid StudentId { get; set; }
        public UserInfo Student { get; set; } = null!;
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public decimal AmountPaid { get; set; } = 0;
        public string? CouponCode { get; set; }
        public decimal? DiscountApplied { get; set; }
        public decimal ProgressPercentage { get; set; } = 0;
        public int TotalTimeSpentMinutes { get; set; } = 0;
        public DateTime? LastAccessedAt { get; set; }
        public ICollection<Progress> Progresses { get; set; } = new List<Progress>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
