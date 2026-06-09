
using Horizon.Domain.Enums;
using System.Reflection.Metadata;
using static System.Collections.Specialized.BitVector32;

namespace Horizon.Domain.Entities
{
    public class Course : AuditableEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? PromoVideoUrl { get; set; }
        public string? Language { get; set; } = "English";
        public CourseLevel Level { get; set; } = CourseLevel.AllLevels;
        public CourseStatus Status { get; set; } = CourseStatus.Draft;
        public decimal Price { get; set; } = 0;
        public decimal? DiscountPrice { get; set; }
        public DateTime? DiscountExpiry { get; set; }
        public string Currency { get; set; } = "USD";
        public int DurationMinutes { get; set; } = 0;
        public bool IsFeatured { get; set; } = false;
        public bool IsLifetimeAccess { get; set; } = true;
        public int? AccessDays { get; set; }
        public string? Prerequisites { get; set; }
        public string? LearningObjectives { get; set; }
        public string? TargetAudience { get; set; }
        public string? WelcomeMessage { get; set; }
        public string? CongratulationMessage { get; set; }
        public double AverageRating { get; set; } = 0;
        public int TotalReviews { get; set; } = 0;
        public int TotalStudents { get; set; } = 0;
        public int TotalLessons { get; set; } = 0;
        public Guid InstructorId { get; set; }
        public UserInfo Instructor { get; set; } = null!;

        public bool HasDiscount => DiscountPrice.HasValue && DiscountPrice > 0 &&
                                   (!DiscountExpiry.HasValue || DiscountExpiry > DateTime.UtcNow);
        public decimal CurrentPrice => HasDiscount ? DiscountPrice!.Value : Price;

        // Navigation properties
        public ICollection<Section> Sections { get; set; } = new List<Section>();
        public ICollection<CourseCategory> CourseCategories { get; set; } = new List<CourseCategory>();
        public ICollection<CourseTag> CourseTags { get; set; } = new List<CourseTag>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public ICollection<CourseInstructor> CourseInstructors { get; set; } = new List<CourseInstructor>();
        public ICollection<Bundle> Bundles { get; set; } = new List<Bundle>();
        public ICollection<BundleCourse> BundleCourses { get; set; } = new List<BundleCourse>();
        public ICollection<LiveSession> LiveSessions { get; set; } = new List<LiveSession>();
    }
}
