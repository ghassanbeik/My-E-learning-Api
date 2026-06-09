

namespace Horizon.Domain.Entities
{
    public class UserInfo : AuditableEntity
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
        public string? Headline { get; set; }
        public string? Website { get; set; }
        public string? Twitter { get; set; }
        public string? LinkedIn { get; set; }
        public string? YouTube { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime? LastLoginAt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        // Navigation properties
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<InstructorProfile> InstructorProfiles { get; set; } = new List<InstructorProfile>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Course> CoursesAsInstructor { get; set; } = new List<Course>();
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Discussion> Discussions { get; set; } = new List<Discussion>();
        public ICollection<DiscussionReply> DiscussionReplies { get; set; } = new List<DiscussionReply>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<InstructorSubscriber> Subscribers { get; set; } = new List<InstructorSubscriber>();
        public ICollection<InstructorSubscriber> Subscriptions { get; set; } = new List<InstructorSubscriber>();
        public ICollection<NotificationPreference> NotificationPreferences { get; set; } = new List<NotificationPreference>();
        public ICollection<LessonNote> LessonNotes { get; set; } = new List<LessonNote>();
        public ICollection<LessonBookmark> LessonBookmarks { get; set; } = new List<LessonBookmark>();
        public ICollection<CourseInstructor> CourseInstructors { get; set; } = new List<CourseInstructor>();
    }
}
