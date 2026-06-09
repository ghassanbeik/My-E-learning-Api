

namespace Horizon.Domain.Entities
{
    public class DiscussionReply : AuditableEntity
    {
        public Guid DiscussionId { get; set; }
        public Discussion Discussion { get; set; } = null!;
        public Guid UserId { get; set; }
        public UserInfo User { get; set; } = null!;
        public string Content { get; set; } = string.Empty;
        public Guid? ParentReplyId { get; set; }
        public DiscussionReply? ParentReply { get; set; }
        public ICollection<DiscussionReply> ChildReplies { get; set; } = new List<DiscussionReply>();
        public int UpvoteCount { get; set; } = 0;
        public bool IsInstructorAnswer { get; set; } = false;
        public ICollection<DiscussionVote> Votes { get; set; } = new List<DiscussionVote>();
    }
}
