

namespace Horizon.Domain.Entities
{
    public class LessonBookmark : BaseEntity
    {
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
        public Guid UserId { get; set; }
        public UserInfo User { get; set; } = null!;
        public int? VideoTimestampSeconds { get; set; }
        public string? Note { get; set; }
    }
}
