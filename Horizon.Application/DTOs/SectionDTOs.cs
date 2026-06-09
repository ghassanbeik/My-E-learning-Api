
namespace Horizon.Application.DTOs
{
    public record SectionDto(
        Guid Id,
        Guid CourseId,
        string Title,
        string? Description,
        int DisplayOrder,
        int DurationMinutes,
        int LessonCount,
        List<LessonDto> Lessons);

    public record CreateSectionDto(
        string Title,
        string? Description,
        int DisplayOrder = 0);

    public record UpdateSectionDto(
        string? Title,
        string? Description,
        int? DisplayOrder);
}
