

namespace Horizon.Domain.Entities
{
    public class CourseInstructor : BaseEntity
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public Guid InstructorId { get; set; }
        public UserInfo Instructor { get; set; } = null!;
        public decimal RevenueSharePercentage { get; set; } = 0;
        public bool IsOwner { get; set; } = false;
    }
}
