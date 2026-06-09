namespace Horizon.Domain.Interfaces.Services.SearchServices
{
    public class SearchResult
    {
        public IEnumerable<CourseSearchItem> Courses { get; set; } = Enumerable.Empty<CourseSearchItem>();
        public IEnumerable<InstructorSearchItem> Instructors { get; set; } = Enumerable.Empty<InstructorSearchItem>();
        public int TotalCourses { get; set; }
        public int TotalInstructors { get; set; }
        public string Query { get; set; } = string.Empty;
    }
}
