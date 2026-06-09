

namespace Horizon.Domain.Entities
{
    public class PlatformAnalytics : BaseEntity
    {
        public DateTime Date { get; set; }
        public int NewUsers { get; set; } = 0;
        public int NewCourses { get; set; } = 0;
        public int TotalEnrollments { get; set; } = 0;
        public decimal TotalRevenue { get; set; } = 0;
        public int ActiveUsers { get; set; } = 0;
        public int NewInstructors { get; set; } = 0;
        public int CoursesPublished { get; set; } = 0;
        public int CertificatesIssued { get; set; } = 0;
    }
}
