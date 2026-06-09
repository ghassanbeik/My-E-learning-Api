

namespace Horizon.Domain.Entities
{
    public class InstructorSubscriber : BaseEntity
    {
        public Guid InstructorId { get; set; }
        public UserInfo Instructor { get; set; } = null!;
        public Guid SubscriberId { get; set; }
        public UserInfo Subscriber { get; set; } = null!;
        public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
        public bool IsNotified { get; set; } = true;
    }
}
