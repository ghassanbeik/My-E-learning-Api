

using Horizon.Domain.Enums;

namespace Horizon.Domain.Entities
{
    public class Discussion : AuditableEntity
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public Guid? LessonId { get; set; }
        public Lesson? Lesson { get; set; }
        public Guid UserId { get; set; }
        public UserInfo User { get; set; } = null!;
        public DiscussionType Type { get; set; } = DiscussionType.General;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsPinned { get; set; } = false;
        public bool IsAnswered { get; set; } = false;
        public Guid? AcceptedReplyId { get; set; }
        public int UpvoteCount { get; set; } = 0;
        public int ReplyCount { get; set; } = 0;
        public ICollection<DiscussionReply> Replies { get; set; } = new List<DiscussionReply>();
        public ICollection<DiscussionVote> Votes { get; set; } = new List<DiscussionVote>();
    }
}
