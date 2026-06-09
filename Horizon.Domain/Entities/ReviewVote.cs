

namespace Horizon.Domain.Entities
{
    public class ReviewVote : BaseEntity
    {
        public Guid ReviewId { get; set; }
        public Review Review { get; set; } = null!;
        public Guid UserId { get; set; }
        public UserInfo User { get; set; } = null!;
        public bool IsHelpful { get; set; } = true;
    }
}
