

namespace Horizon.Domain.Entities
{
    public class Section : AuditableEntity
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public int DurationMinutes { get; set; } = 0;
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }

}
