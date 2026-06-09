
namespace Horizon.Application.DTOs
{
    public record LiveSessionDto(
        Guid Id,
        Guid CourseId,
        string CourseTitle,
        Guid InstructorId,
        string InstructorName,
        string Title,
        string? Description,
        DateTime ScheduledAt,
        int DurationMinutes,
        string? MeetingUrl,
        string? RecordingUrl,
        bool IsCompleted,
        int AttendeeCount);

    public record CreateLiveSessionDto(
        Guid CourseId,
        string Title,
        string? Description,
        DateTime ScheduledAt,
        int DurationMinutes,
        string? MeetingUrl);
}
