

namespace Horizon.Application.DTOs
{
    public record CategoryDto(
         Guid Id,
         string Name,
         string? Description,
         string? IconUrl,
         string? Color,
         Guid? ParentId,
         bool IsFeatured,
         int DisplayOrder,
         int CourseCount,
         List<CategoryDto>? SubCategories);

    public record CreateCategoryDto(
        string Name,
        string? Description,
        string? IconUrl,
        string? Color,
        Guid? ParentId,
        bool IsFeatured,
        int DisplayOrder);

    public record UpdateCategoryDto(
        string? Name,
        string? Description,
        string? IconUrl,
        string? Color,
        bool? IsFeatured,
        int? DisplayOrder);
}
