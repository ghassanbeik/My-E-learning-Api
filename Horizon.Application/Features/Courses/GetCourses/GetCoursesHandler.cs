

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Repositories;
using MediatR;

namespace Horizon.Application.Features.Courses.GetCourses
{
    public class GetCoursesHandler : IRequestHandler<GetCoursesQuery, Result<PagedResponse<CourseListDto>>>
    {
        private readonly IUnitOfWork _uow;

        public GetCoursesHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<PagedResponse<CourseListDto>>> Handle(GetCoursesQuery request, CancellationToken ct)
        {
            var s = request.Search;
            var result = await _uow.Courses.SearchCoursesAsync(new CourseSearchParams
            {
                Query = s.Query,
                CategoryId = s.CategoryId,
                TagId = s.TagId,
                Level = s.Level != null ? Enum.Parse<CourseLevel>(s.Level) : null,
                MinPrice = s.MinPrice,
                MaxPrice = s.MaxPrice,
                MinRating = s.MinRating,
                Language = s.Language,
                IsFeatured = s.IsFeatured,
                IsFree = s.IsFree,
                Page = s.Page,
                PageSize = s.PageSize,
                SortBy = s.SortBy,
                Descending = s.Descending,
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
