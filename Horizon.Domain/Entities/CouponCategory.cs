

namespace Horizon.Domain.Entities
{
    public class CouponCategory : BaseEntity
    {
        public Guid CouponId { get; set; }
        public Coupon Coupon { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
