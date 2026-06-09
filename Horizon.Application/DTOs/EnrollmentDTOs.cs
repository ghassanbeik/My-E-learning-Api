
namespace Horizon.Application.DTOs
{
    public record EnrollmentDto(
        Guid Id,
        Guid CourseId,
        string CourseTitle,
        string? CourseThumbnail,
        string InstructorName,
        string Status,
        decimal AmountPaid,
        decimal ProgressPercentage,
        int CompletedLessons,
        int TotalLessons,
        DateTime EnrolledAt,
        DateTime? CompletedAt,
        DateTime? ExpiresAt,
        DateTime? LastAccessedAt);

    public record EnrollmentDetailDto(
        Guid Id,
        CourseDetailDto Course,
        string Status,
        decimal AmountPaid,
        decimal ProgressPercentage,
        int TotalTimeSpentMinutes,
        DateTime EnrolledAt,
        DateTime? CompletedAt,
        List<ProgressDto> LessonProgress);

    public record ProgressDto(
        Guid Id,
        Guid LessonId,
        string LessonTitle,
        bool IsCompleted,
        DateTime? CompletedAt,
        int TimeSpentMinutes,
        int? VideoWatchedSeconds,
        int? VideoTotalSeconds);

    public record UpdateProgressDto(
        bool IsCompleted,
        int? VideoWatchedSeconds,
        int? VideoTotalSeconds,
        int? TimeSpentMinutes);

}
