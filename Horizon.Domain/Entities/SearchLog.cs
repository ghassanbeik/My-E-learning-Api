

namespace Horizon.Domain.Entities
{
    public class SearchLog : BaseEntity
    {
        public string Query { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
        public UserInfo? User { get; set; }
        public string? Filters { get; set; }
        public int ResultsCount { get; set; } = 0;
        public DateTime SearchedAt { get; set; } = DateTime.UtcNow;
        public string? IpAddress { get; set; }
    }
}
