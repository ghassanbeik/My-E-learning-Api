

namespace Horizon.Domain.Entities
{
    public class BundleCourse : BaseEntity
    {
        public Guid BundleId { get; set; }
        public Bundle Bundle { get; set; } = null!;
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public int DisplayOrder { get; set; } = 0;
    }
}
