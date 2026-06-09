
namespace Horizon.Application.Common
{
    public class PagedRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; }
        public bool Descending { get; set; } = true;
    }
}
