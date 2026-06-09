

namespace Horizon.Application.DTOs
{
    public record AssignmentDto(
        Guid Id,
        Guid LessonId,
        string Title,
        string Description,
        string? Instructions,
        int TotalPoints,
        DateTime? DueDate,
        bool AllowLateSubmission,
        bool IsSubmitted,
        int? Score,
        bool IsGraded,
        DateTime? SubmittedAt);

    public record CreateAssignmentDto(
        Guid LessonId,
        string Title,
        string Description,
        string? Instructions,
        int TotalPoints,
        DateTime? DueDate,
        bool AllowLateSubmission,
        int LatePenaltyPercentage,
        int? TimeLimitHours);

    public record SubmitAssignmentDto(string? SubmissionText, string? FileUrl, string? FileName);

    public record GradeAssignmentDto(int Score, string? Feedback);

    public record AssignmentSubmissionDto(
        Guid Id,
        Guid AssignmentId,
        Guid StudentId,
        string StudentName,
        string? SubmissionText,
        string? FileUrl,
        string? FileName,
        DateTime? SubmittedAt,
        int? Score,
        string? Feedback,
        bool IsGraded,
        string? GradedByName,
        bool IsLate);
}
