

namespace Horizon.Domain.Entities
{
    public class LessonNote : BaseEntity
    {
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
        public Guid UserId { get; set; }
        public UserInfo User { get; set; } = null!;
        public string Content { get; set; } = string.Empty;
        public int? VideoTimestampSeconds { get; set; }
    }
}
