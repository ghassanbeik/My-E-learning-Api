

namespace Horizon.Application.DTOs
{
    public record LessonDto(
         Guid Id,
         Guid SectionId,
         string Title,
         string? Description,
         string ContentType,
         int DisplayOrder,
         int DurationMinutes,
         bool IsPreview,
         bool IsDownloadable,
         string? VideoUrl,
         string? ArticleContent,
         string? ResourceUrl,
         bool IsCompleted,
         int? VideoWatchedSeconds,
         int? VideoTotalSeconds);

    public record CreateLessonDto(
        string Title,
        string? Description,
        string ContentType,
        int DisplayOrder,
        int DurationMinutes,
        bool IsPreview,
        bool IsDownloadable,
        string? VideoUrl,
        string? ArticleContent,
        string? ResourceUrl);

    public record UpdateLessonDto(
        string? Title,
        string? Description,
        string? ContentType,
        int? DisplayOrder,
        int? DurationMinutes,
        bool? IsPreview,
        bool? IsDownloadable,
        string? VideoUrl,
        string? ArticleContent,
        string? ResourceUrl);

    public record LessonNoteDto(
        Guid Id,
        Guid LessonId,
        string Content,
        int? VideoTimestampSeconds,
        DateTime CreatedAt);

    public record CreateLessonNoteDto(string Content, int? VideoTimestampSeconds);

    public record LessonBookmarkDto(
        Guid Id,
        Guid LessonId,
        string LessonTitle,
        int? VideoTimestampSeconds,
        string? Note,
        DateTime CreatedAt);

    public record CreateLessonBookmarkDto(int? VideoTimestampSeconds, string? Note);
}
