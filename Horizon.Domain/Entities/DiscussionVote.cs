

namespace Horizon.Domain.Entities
{
    public class DiscussionVote : BaseEntity
    {
        public Guid? DiscussionId { get; set; }
        public Discussion? Discussion { get; set; }
        public Guid? ReplyId { get; set; }
        public DiscussionReply? Reply { get; set; }
        public Guid UserId { get; set; }
        public UserInfo User { get; set; } = null!;
        public bool IsUpvote { get; set; } = true;
    }
}
