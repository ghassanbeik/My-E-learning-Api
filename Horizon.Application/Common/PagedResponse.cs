
namespace Horizon.Application.Common
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;

        public static PagedResponse<T> From(IEnumerable<T> items, int totalCount, int page, int pageSize)
            => new() { Items = items, TotalCount = totalCount, Page = page, PageSize = pageSize };
    }
}
