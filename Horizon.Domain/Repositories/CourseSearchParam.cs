
using Horizon.Domain.Enums;

namespace Horizon.Domain.Repositories
{
    public class CourseSearchParams
    {
        public string? Query { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? TagId { get; set; }
        public CourseLevel? Level { get; set; }
        public CourseStatus? Status { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public double? MinRating { get; set; }
        public string? Language { get; set; }
        public bool? IsFeatured { get; set; }
        public bool? IsFree { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "CreatedAt";
        public bool Descending { get; set; } = true;
    }
}
