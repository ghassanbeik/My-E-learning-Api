

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.CacheServices;
using MediatR;

namespace Horizon.Application.Features.Courses.GetFeaturedCourses
{
    public class GetFeaturedCoursesHandler : IRequestHandler<GetFeaturedCoursesQuery, Result<List<CourseListDto>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cache;

        public GetFeaturedCoursesHandler(IUnitOfWork uow, ICacheService cache) { _uow = uow; _cache = cache; }

        public async Task<Result<List<CourseListDto>>> Handle(GetFeaturedCoursesQuery request, CancellationToken ct)
        {
            var courses = await _cache.GetOrSetAsync(
                CacheKeys.FeaturedCourses(),
                async () => (await _uow.Courses.GetFeaturedCoursesAsync(request.Count, ct)).ToList(),
                TimeSpan.FromMinutes(30), ct);

            return Result<List<CourseListDto>>.Success(courses!.Select(c => new CourseListDto(
                c.Id, c.Title, c.Subtitle, c.ShortDescription, c.ThumbnailUrl,
                c.Instructor.FullName, c.InstructorId, c.Instructor.AvatarUrl,
                c.Level.ToString(), c.Status.ToString(), c.Price, c.DiscountPrice,
                c.CurrentPrice, c.HasDiscount, c.AverageRating, c.TotalReviews,
                c.TotalStudents, c.TotalLessons, c.DurationMinutes, c.IsFeatured,
                c.CourseCategories.Select(cc => cc.Category.Name).ToList(),
                c.CourseTags.Select(ct2 => ct2.Tag.Name).ToList(),
                c.CreatedAt)).ToList());
        }
    }
}
