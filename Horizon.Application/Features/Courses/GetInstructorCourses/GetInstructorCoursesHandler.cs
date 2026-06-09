
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Courses.GetInstructorCourses
{
    public class GetInstructorCoursesHandler : IRequestHandler<GetInstructorCoursesQuery, Result<List<CourseListDto>>>
    {
        private readonly IUnitOfWork _uow;

        public GetInstructorCoursesHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<List<CourseListDto>>> Handle(GetInstructorCoursesQuery request, CancellationToken ct)
        {
            var courses = await _uow.Courses.GetByInstructorAsync(request.InstructorId, ct);
            return Result<List<CourseListDto>>.Success(courses.Select(c => new CourseListDto(
                c.Id, c.Title, c.Subtitle, c.ShortDescription, c.ThumbnailUrl,
                c.Instructor?.FullName ?? string.Empty, c.InstructorId, c.Instructor?.AvatarUrl,
                c.Level.ToString(), c.Status.ToString(), c.Price, c.DiscountPrice,
                c.CurrentPrice, c.HasDiscount, c.AverageRating, c.TotalReviews,
                c.TotalStudents, c.TotalLessons, c.DurationMinutes, c.IsFeatured,
                c.CourseCategories.Select(cc => cc.Category.Name).ToList(),
                c.CourseTags.Select(ct2 => ct2.Tag.Name).ToList(),
                c.CreatedAt)).ToList());
        }
    }
}
