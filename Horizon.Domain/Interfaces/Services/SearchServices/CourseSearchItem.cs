namespace Horizon.Domain.Interfaces.Services.SearchServices
{
    public class CourseSearchItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public decimal Price { get; set; }
        public double Rating { get; set; }
        public int TotalStudents { get; set; }
    }
}
