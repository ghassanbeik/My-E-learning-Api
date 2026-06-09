

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Events.CourseEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.CacheServices;
using MediatR;

namespace Horizon.Application.Features.Courses.CreateCourse
{
    public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, Result<CourseListDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;
        private readonly ICacheService _cache;

        public CreateCourseHandler(IUnitOfWork uow, IEventBus eventBus, ICacheService cache)
        {
            _uow = uow;
            _eventBus = eventBus;
            _cache = cache;
        }

        public async Task<Result<CourseListDto>> Handle(CreateCourseCommand request, CancellationToken ct)
        {
            var instructor = await _uow.Users.GetByIdAsync(request.InstructorId, ct);
            if (instructor == null) return Result<CourseListDto>.NotFound("Instructor not found.");

            var course = new Course
            {
                InstructorId = request.InstructorId,
                Title = request.Dto.Title,
                Subtitle = request.Dto.Subtitle,
                Description = request.Dto.Description,
                ShortDescription = request.Dto.ShortDescription,
                Level = Enum.Parse<CourseLevel>(request.Dto.Level),
                Status = CourseStatus.Draft,
                Price = request.Dto.Price,
                DiscountPrice = request.Dto.DiscountPrice,
                DiscountExpiry = request.Dto.DiscountExpiry,
                Language = request.Dto.Language ?? "English",
                Prerequisites = request.Dto.Prerequisites,
                LearningObjectives = request.Dto.LearningObjectives,
                TargetAudience = request.Dto.TargetAudience,
            };

            await _uow.Courses.AddAsync(course, ct);
            await _uow.SaveChangesAsync(ct);

            // Add categories
            foreach (var catId in request.Dto.CategoryIds)
                await _uow.CourseCategories.AddAsync(new CourseCategory { CourseId = course.Id, CategoryId = catId }, ct);

            // Add tags
            if (request.Dto.TagIds?.Any() == true)
                foreach (var tagId in request.Dto.TagIds)
                    await _uow.CourseTags.AddAsync(new CourseTag { CourseId = course.Id, TagId = tagId }, ct);

            await _uow.SaveChangesAsync(ct);
            await _cache.RemoveByPrefixAsync("courses:", ct);

            await _eventBus.PublishAsync(new CourseCreatedEvent
            {
                CourseId = course.Id,
                InstructorId = request.InstructorId,
                CourseTitle = course.Title,
            }, ct);

            return Result<CourseListDto>.Success(MapToListDto(course, instructor), 201);
        }

        private static CourseListDto MapToListDto(Course c, UserInfo instructor) => new(
            c.Id, c.Title, c.Subtitle, c.ShortDescription, c.ThumbnailUrl,
            instructor.FullName, instructor.Id, instructor.AvatarUrl,
            c.Level.ToString(), c.Status.ToString(), c.Price, c.DiscountPrice,
            c.CurrentPrice, c.HasDiscount, c.AverageRating, c.TotalReviews,
            c.TotalStudents, c.TotalLessons, c.DurationMinutes, c.IsFeatured,
            new(), new(), c.CreatedAt);
    }

}
