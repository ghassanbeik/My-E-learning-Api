
namespace Horizon.Domain.Entities
{
    public class CartItem : BaseEntity
    {
        public Guid UserId { get; set; }
        public UserInfo User { get; set; } = null!;
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public string? CouponCode { get; set; }
        public decimal? DiscountAmount { get; set; }
    }
}
