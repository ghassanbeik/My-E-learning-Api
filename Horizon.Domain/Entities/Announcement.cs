

namespace Horizon.Domain.Entities
{
    public class Announcement : AuditableEntity
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public Guid InstructorId { get; set; }
        public UserInfo Instructor { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsPinned { get; set; } = false;
        public DateTime? PinnedUntil { get; set; }
        public ICollection<AnnouncementRead> ReadBy { get; set; } = new List<AnnouncementRead>();
    }
}
