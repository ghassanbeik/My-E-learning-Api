

namespace Horizon.Domain.Entities
{
    public class Wishlist : BaseEntity
    {
        public Guid UserId { get; set; }
        public UserInfo User { get; set; } = null!;
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
    }
}
