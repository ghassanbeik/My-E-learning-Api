

namespace Horizon.Domain.Entities
{
    public class Bundle : AuditableEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public decimal Price { get; set; } = 0;
        public decimal? DiscountPrice { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<BundleCourse> BundleCourses { get; set; } = new List<BundleCourse>();
    }
}
