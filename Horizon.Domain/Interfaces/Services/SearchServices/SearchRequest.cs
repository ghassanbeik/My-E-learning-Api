namespace Horizon.Domain.Interfaces.Services.SearchServices
{
    public class SearchRequest
    {
        public string Query { get; set; } = string.Empty;
        public SearchCategory Category { get; set; } = SearchCategory.All;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public Guid? UserId { get; set; }
    }
    public enum SearchCategory { All, Courses, Instructors }

}
