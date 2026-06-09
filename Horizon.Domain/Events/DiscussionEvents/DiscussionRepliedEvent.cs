

namespace Horizon.Domain.Events.DiscussionEvents
{
    public class DiscussionRepliedEvent : DomainEvent
    {
        public Guid ReplyId { get; init; }
        public Guid DiscussionId { get; init; }
        public Guid DiscussionAuthorId { get; init; }
        public Guid ReplierId { get; init; }
        public string ReplierName { get; init; } = string.Empty;
        public bool IsInstructorReply { get; init; }
    }
}
