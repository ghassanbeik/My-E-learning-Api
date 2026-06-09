

namespace Horizon.Domain.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int UsageCount { get; set; } = 0;
        public ICollection<CourseTag> CourseTags { get; set; } = new List<CourseTag>();
    }
}
