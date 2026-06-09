

using Horizon.Domain.Enums;

namespace Horizon.Domain.Entities
{
    public class Coupon : AuditableEntity
    {
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public CouponType Type { get; set; } = CouponType.Percentage;
        public decimal Value { get; set; } = 0;
        public decimal? MaxDiscountAmount { get; set; }
        public decimal? MinOrderAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? MaxUses { get; set; }
        public int CurrentUses { get; set; } = 0;
        public int? MaxUsesPerUser { get; set; } = 1;
        public bool IsActive { get; set; } = true;
        public ICollection<CouponCourse> ApplicableCourses { get; set; } = new List<CouponCourse>();
        public ICollection<CouponCategory> ApplicableCategories { get; set; } = new List<CouponCategory>();
        public ICollection<CouponUsage> Usages { get; set; } = new List<CouponUsage>();
    }
}
