

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Analytics.GetCourseAnalytics
{
    public class GetCourseAnalyticsHandler : IRequestHandler<GetCourseAnalyticsQuery, Result<CourseAnalyticsDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetCourseAnalyticsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<CourseAnalyticsDto>> Handle(GetCourseAnalyticsQuery request, CancellationToken ct)
        {
            var course = await _uow.Courses.GetByIdAsync(request.CourseId, ct);
            if (course == null)
                return Result<CourseAnalyticsDto>.NotFound("Course not found.");

            if (course.InstructorId != request.InstructorId)
                return Result<CourseAnalyticsDto>.Forbidden("You do not own this course.");

            var dailyStats = await _uow.CourseAnalytics
                .GetByCourseAndDateRangeAsync(request.CourseId, request.From, request.To, ct);

            var totalEnrollments = await _uow.Enrollments.GetEnrollmentCountAsync(request.CourseId, ct);
            var totalRevenue = await _uow.Payments.GetInstructorEarningsAsync(request.InstructorId, request.From, request.To, ct);
            var avgRating = await _uow.Reviews.GetAverageRatingAsync(request.CourseId, ct);
            var totalReviews = await _uow.Reviews.CountAsync(
                r => r.CourseId == request.CourseId &&
                     r.Status == Domain.Enums.ReviewStatus.Approved, ct);

            var dailyDtos = dailyStats.Select(d => new DailyAnalyticsDto(
                d.Date,
                d.NewEnrollments,
                d.Completions,
                d.Revenue,
                d.UniqueVisitors,
                d.VideoViews)).ToList();

            return Result<CourseAnalyticsDto>.Success(new CourseAnalyticsDto(
                course.Id,
                course.Title,
                totalEnrollments,
                dailyStats.Sum(d => d.Completions),
                totalReviews,
                totalRevenue,
                avgRating,
                dailyStats.Any() ? dailyStats.Average(d => d.AverageProgress) : 0,
                dailyDtos));
        }
    }
}
