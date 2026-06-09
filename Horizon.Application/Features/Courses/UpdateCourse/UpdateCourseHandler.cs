

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.CacheServices;
using MediatR;

namespace Horizon.Application.Features.Courses.UpdateCourse
{
    public class UpdateCourseHandler : IRequestHandler<UpdateCourseCommand, Result<CourseListDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cache;

        public UpdateCourseHandler(IUnitOfWork uow, ICacheService cache) { _uow = uow; _cache = cache; }

        public async Task<Result<CourseListDto>> Handle(UpdateCourseCommand request, CancellationToken ct)
        {
            var course = await _uow.Courses.GetWithDetailsAsync(request.CourseId, ct);
            if (course == null) return Result<CourseListDto>.NotFound("Course not found.");
            if (course.InstructorId != request.InstructorId) return Result<CourseListDto>.Forbidden();

            var dto = request.Dto;
            if (dto.Title != null) course.Title = dto.Title;
            if (dto.Subtitle != null) course.Subtitle = dto.Subtitle;
            if (dto.Description != null) course.Description = dto.Description;
            if (dto.ShortDescription != null) course.ShortDescription = dto.ShortDescription;
            if (dto.Level != null) course.Level = Enum.Parse<CourseLevel>(dto.Level);
            if (dto.Price != null) course.Price = dto.Price.Value;
            if (dto.DiscountPrice != null) course.DiscountPrice = dto.DiscountPrice;
            if (dto.DiscountExpiry != null) course.DiscountExpiry = dto.DiscountExpiry;
            if (dto.Language != null) course.Language = dto.Language;
            if (dto.Prerequisites != null) course.Prerequisites = dto.Prerequisites;
            if (dto.LearningObjectives != null) course.LearningObjectives = dto.LearningObjectives;
            if (dto.TargetAudience != null) course.TargetAudience = dto.TargetAudience;
            if (dto.WelcomeMessage != null) course.WelcomeMessage = dto.WelcomeMessage;
            if (dto.CongratulationMessage != null) course.CongratulationMessage = dto.CongratulationMessage;

            await _uow.Courses.UpdateAsync(course);
            await _uow.SaveChangesAsync(ct);
            await _cache.RemoveAsync(CacheKeys.Course(course.Id), ct);

            return Result<CourseListDto>.Success(new CourseListDto(
                course.Id, course.Title, course.Subtitle, course.ShortDescription, course.ThumbnailUrl,
                course.Instructor.FullName, course.InstructorId, course.Instructor.AvatarUrl,
                course.Level.ToString(), course.Status.ToString(), course.Price, course.DiscountPrice,
                course.CurrentPrice, course.HasDiscount, course.AverageRating, course.TotalReviews,
                course.TotalStudents, course.TotalLessons, course.DurationMinutes, course.IsFeatured,
                course.CourseCategories.Select(cc => cc.Category.Name).ToList(),
                course.CourseTags.Select(ct2 => ct2.Tag.Name).ToList(),
                course.CreatedAt));
        }
    }

}
