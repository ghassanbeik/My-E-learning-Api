

namespace Horizon.Domain.Entities
{
    public class ReviewResponse : AuditableEntity
    {
        public Guid ReviewId { get; set; }
        public Review Review { get; set; } = null!;
        public Guid ResponderId { get; set; }
        public UserInfo Responder { get; set; } = null!;
        public string Response { get; set; } = string.Empty;
    }
}
