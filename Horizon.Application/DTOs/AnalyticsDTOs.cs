

namespace Horizon.Application.DTOs
{
    public record CourseAnalyticsDto(
        Guid CourseId,
        string CourseTitle,
        int TotalEnrollments,
        int TotalCompletions,
        int TotalReviews,
        decimal TotalRevenue,
        double AverageRating,
        double AverageProgress,
        List<DailyAnalyticsDto> DailyStats);

    public record DailyAnalyticsDto(
        DateTime Date,
        int NewEnrollments,
        int Completions,
        decimal Revenue,
        int UniqueVisitors,
        int VideoViews);

    public record InstructorDashboardDto(
        decimal TotalEarnings,
        decimal PendingEarnings,
        int TotalStudents,
        int TotalCourses,
        double AverageRating,
        int TotalReviews,
        List<CourseListDto> RecentCourses,
        List<EnrollmentDto> RecentEnrollments);

    public record PlatformStatsDto(
        int TotalUsers,
        int TotalInstructors,
        int TotalStudents,
        int TotalCourses,
        int TotalEnrollments,
        decimal TotalRevenue,
        int CertificatesIssued);

}
