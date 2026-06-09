

namespace Horizon.Domain.Entities
{
    public class InstructorProfile : AuditableEntity
    {
        public Guid UserId { get; set; }
        public UserInfo User { get; set; } = null!;
        public string? TeachingExperience { get; set; }
        public string? Education { get; set; }
        public decimal AverageRating { get; set; } = 0;
        public int TotalStudents { get; set; } = 0;
        public int TotalCourses { get; set; } = 0;
        public bool IsVerified { get; set; } = false;
        public string? VerificationDocument { get; set; }
        public string? PayoutAccount { get; set; }
        public decimal TotalEarnings { get; set; } = 0;
        public decimal PendingEarnings { get; set; } = 0;
    }
}
