

namespace Horizon.Domain.Entities
{
    public class CourseAnalytics : BaseEntity
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public DateTime Date { get; set; }
        public int NewEnrollments { get; set; } = 0;
        public int Completions { get; set; } = 0;
        public int Reviews { get; set; } = 0;
        public decimal Revenue { get; set; } = 0;
        public decimal Refunds { get; set; } = 0;
        public int UniqueVisitors { get; set; } = 0;
        public int VideoViews { get; set; } = 0;
        public int QuizAttempts { get; set; } = 0;
        public int AssignmentSubmissions { get; set; } = 0;
        public double AverageProgress { get; set; } = 0;
        public double AverageRating { get; set; } = 0;
        public int WishlistAdds { get; set; } = 0;
        public int CartAdds { get; set; } = 0;
    }
}
