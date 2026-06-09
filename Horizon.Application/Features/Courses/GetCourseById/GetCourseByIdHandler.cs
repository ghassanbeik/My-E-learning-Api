

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.CacheServices;
using MediatR;

namespace Horizon.Application.Features.Courses.GetCourseById
{
    public class GetCourseByIdHandler : IRequestHandler<GetCourseByIdQuery, Result<CourseDetailDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cache;

        public GetCourseByIdHandler(IUnitOfWork uow, ICacheService cache) { _uow = uow; _cache = cache; }

        public async Task<Result<CourseDetailDto>> Handle(GetCourseByIdQuery request, CancellationToken ct)
        {
            var course = await _cache.GetOrSetAsync(
                CacheKeys.Course(request.CourseId),
                async () => await _uow.Courses.GetWithDetailsAsync(request.CourseId, ct),
                TimeSpan.FromMinutes(10), ct);

            if (course == null) return Result<CourseDetailDto>.NotFound("Course not found.");

            var isEnrolled = request.UserId.HasValue && await _uow.Enrollments.IsEnrolledAsync(request.UserId.Value, course.Id, ct);
            var isInWishlist = request.UserId.HasValue && await _uow.Wishlists.ExistsAsync(request.UserId.Value, course.Id, ct);
            var isInCart = request.UserId.HasValue && await _uow.CartItems.ExistsAsync(request.UserId.Value, course.Id, ct);
            double? progress = null;

            if (isEnrolled && request.UserId.HasValue)
            {
                var enrollment = await _uow.Enrollments.GetByStudentAndCourseAsync(request.UserId.Value, course.Id, ct);
                progress = (double?)enrollment?.ProgressPercentage;
            }

            return Result<CourseDetailDto>.Success(MapToDetailDto(course, isEnrolled, isInWishlist, isInCart, progress));
        }

        private static CourseDetailDto MapToDetailDto(Course c, bool isEnrolled, bool isInWishlist, bool isInCart, double? progress) => new(
            c.Id, c.Title, c.Subtitle, c.Description, c.ShortDescription, c.ThumbnailUrl, c.PromoVideoUrl,
            c.Language, c.Level.ToString(), c.Status.ToString(), c.Price, c.DiscountPrice, c.CurrentPrice,
            c.HasDiscount, c.Currency, c.DurationMinutes, c.IsFeatured, c.IsLifetimeAccess, c.AccessDays,
            c.Prerequisites, c.LearningObjectives, c.TargetAudience, c.WelcomeMessage,
            c.AverageRating, c.TotalReviews, c.TotalStudents, c.TotalLessons,
            new InstructorDto(c.Instructor.Id, c.Instructor.FullName, c.Instructor.Email, c.Instructor.AvatarUrl,
                c.Instructor.Headline, c.Instructor.Bio, 0, 0, 0, 0, 0, false),
            c.CourseCategories.Select(cc => new CategoryDto(cc.Category.Id, cc.Category.Name, null, null, null, null, false, 0, 0, null)).ToList(),
            c.CourseTags.Select(ct => new TagDto(ct.Tag.Id, ct.Tag.Name, null, ct.Tag.UsageCount)).ToList(),
            c.Sections.OrderBy(s => s.DisplayOrder).Select(s => new SectionDto(
                s.Id, s.CourseId, s.Title, s.Description, s.DisplayOrder, s.DurationMinutes,
                s.Lessons.Count,
                s.Lessons.OrderBy(l => l.DisplayOrder).Select(l => new LessonDto(
                    l.Id, l.SectionId, l.Title, l.Description, l.ContentType.ToString(),
                    l.DisplayOrder, l.DurationMinutes, l.IsPreview, l.IsDownloadable,
                    isEnrolled ? l.VideoUrl : null,
                    isEnrolled ? l.ArticleContent : null,
                    isEnrolled ? l.ResourceUrl : null,
                    false, null, null)).ToList())).ToList(),
            c.Reviews.Take(5).Select(r => new ReviewDto(
                r.Id, r.CourseId, c.Title, r.StudentId, r.Student.FullName, r.Student.AvatarUrl,
                r.Rating, r.Comment, r.Status.ToString(), r.HelpfulCount, new(), r.CreatedAt)).ToList(),
            isEnrolled, isInWishlist, isInCart, progress, c.CreatedAt);
    }

}
