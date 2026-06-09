

namespace Horizon.Application.DTOs
{
    public record BundleDto(
         Guid Id,
         string Title,
         string? Description,
         string? ThumbnailUrl,
         decimal Price,
         decimal? DiscountPrice,
         bool IsActive,
         List<CourseListDto> Courses);

    public record CreateBundleDto(
        string Title,
        string? Description,
        decimal Price,
        decimal? DiscountPrice,
        List<Guid> CourseIds);
}
