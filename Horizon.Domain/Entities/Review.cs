

using Horizon.Domain.Enums;

namespace Horizon.Domain.Entities
{
    public class Review : AuditableEntity
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public Guid StudentId { get; set; }
        public UserInfo Student { get; set; } = null!;
        public int Rating { get; set; } = 5;
        public string? Comment { get; set; }
        public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
        public int HelpfulCount { get; set; } = 0;
        public ICollection<ReviewResponse> Responses { get; set; } = new List<ReviewResponse>();
        public ICollection<ReviewVote> Votes { get; set; } = new List<ReviewVote>();
    }
}
