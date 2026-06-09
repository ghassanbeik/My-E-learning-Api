
namespace Horizon.Domain.Entities
{
    public class CouponUsage : BaseEntity
    {
        public Guid CouponId { get; set; }
        public Coupon Coupon { get; set; } = null!;
        public Guid UserId { get; set; }
        public UserInfo User { get; set; } = null!;
        public Guid? EnrollmentId { get; set; }
        public Enrollment? Enrollment { get; set; }
        public DateTime UsedAt { get; set; } = DateTime.UtcNow;
        public decimal DiscountAmount { get; set; } = 0;
    }
}
