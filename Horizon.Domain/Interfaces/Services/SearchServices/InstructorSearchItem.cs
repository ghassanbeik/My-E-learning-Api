namespace Horizon.Domain.Interfaces.Services.SearchServices
{
    public class InstructorSearchItem
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public string? Headline { get; set; }
        public int TotalStudents { get; set; }
        public decimal AverageRating { get; set; }
    }
}
