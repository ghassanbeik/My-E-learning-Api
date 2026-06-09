

namespace Horizon.Domain.Entities
{
    public class CouponCourse : BaseEntity
    {
        public Guid CouponId { get; set; }
        public Coupon Coupon { get; set; } = null!;
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }
}
