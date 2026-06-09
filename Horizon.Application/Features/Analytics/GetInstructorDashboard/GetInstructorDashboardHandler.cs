

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Analytics.GetInstructorDashboard
{
    public class GetInstructorDashboardHandler : IRequestHandler<GetInstructorDashboardQuery, Result<InstructorDashboardDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetInstructorDashboardHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<InstructorDashboardDto>> Handle(GetInstructorDashboardQuery request, CancellationToken ct)
        {
            var profile = await _uow.InstructorProfiles.GetByUserIdAsync(request.InstructorId, ct);
            if (profile == null) return Result<InstructorDashboardDto>.NotFound("Instructor not found.");

            var courses = await _uow.Courses.GetByInstructorAsync(request.InstructorId, ct);
            var recentCourses = courses.Take(5).Select(c => new CourseListDto(
                c.Id, c.Title, c.Subtitle, c.ShortDescription, c.ThumbnailUrl,
                string.Empty, c.InstructorId, null, c.Level.ToString(), c.Status.ToString(),
                c.Price, c.DiscountPrice, c.CurrentPrice, c.HasDiscount,
                c.AverageRating, c.TotalReviews, c.TotalStudents, c.TotalLessons,
                c.DurationMinutes, c.IsFeatured, new(), new(), c.CreatedAt)).ToList();

            return Result<InstructorDashboardDto>.Success(new InstructorDashboardDto(
                profile.TotalEarnings, profile.PendingEarnings,
                profile.TotalStudents, profile.TotalCourses,
                (double)profile.AverageRating, 0,
                recentCourses, new()));
        }
    }
}
