

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Repositories;
using MediatR;

namespace Horizon.Application.Features.Courses.GetCoursesByCategory
{
    public class GetCoursesByCategoryHandler
        : IRequestHandler<GetCoursesByCategoryQuery, Result<PagedResponse<CourseListDto>>>
    {
        private readonly IUnitOfWork _uow;

        public GetCoursesByCategoryHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<PagedResponse<CourseListDto>>> Handle(
            GetCoursesByCategoryQuery request, CancellationToken ct)
        {
            var result = await _uow.Courses.SearchCoursesAsync(new CourseSearchParams
            {
                CategoryId = request.CategoryId,
                Page = request.Page,
                PageSize = request.PageSize,
                Status = CourseStatus.Published,
            }, ct);

            var items = result.Items.Select(c => new CourseListDto(
                c.Id, c.Title, c.Subtitle, c.ShortDescription, c.ThumbnailUrl,
                c.Instructor.FullName, c.InstructorId, c.Instructor.AvatarUrl,
                c.Level.ToString(), c.Status.ToString(), c.Price, c.DiscountPrice,
                c.CurrentPrice, c.HasDiscount, c.AverageRating, c.TotalReviews,
                c.TotalStudents, c.TotalLessons, c.DurationMinutes, c.IsFeatured,
                c.CourseCategories.Select(cc => cc.Category.Name).ToList(),
                c.CourseTags.Select(ct2 => ct2.Tag.Name).ToList(),
                c.CreatedAt)).ToList();

            return Result<PagedResponse<CourseListDto>>.Success(
                PagedResponse<CourseListDto>.From(items, result.TotalCount, result.PageSize, result.PageSize));
        }
    }
}
