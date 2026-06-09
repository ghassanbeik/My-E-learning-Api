

namespace Horizon.Domain.Entities
{
    public class CourseCategory : BaseEntity
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public bool IsPrimary { get; set; } = false;
    }
}
