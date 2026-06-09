

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using MediatR;
using System.Net.Mime;

namespace Horizon.Application.Features.Lessons.CreateLesson
{
    public class CreateLessonHandler : IRequestHandler<CreateLessonCommand, Result<LessonDto>>
    {
        private readonly IUnitOfWork _uow;
        public CreateLessonHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<LessonDto>> Handle(CreateLessonCommand request, CancellationToken ct)
        {
            var section = await _uow.Sections.GetByIdAsync(request.SectionId, ct);
            if (section == null) return Result<LessonDto>.NotFound("Section not found.");

            var course = await _uow.Courses.GetByIdAsync(section.CourseId, ct);
            if (course?.InstructorId != request.InstructorId) return Result<LessonDto>.Forbidden();

            var lesson = new Lesson
            {
                SectionId = request.SectionId,
                Title = request.Dto.Title,
                Description = request.Dto.Description,
                ContentType = Enum.Parse<LessonContentType>(request.Dto.ContentType),
                DisplayOrder = request.Dto.DisplayOrder,
                DurationMinutes = request.Dto.DurationMinutes,
                IsPreview = request.Dto.IsPreview,
                IsDownloadable = request.Dto.IsDownloadable,
                VideoUrl = request.Dto.VideoUrl,
                ArticleContent = request.Dto.ArticleContent,
                ResourceUrl = request.Dto.ResourceUrl,
            };

            await _uow.Lessons.AddAsync(lesson, ct);
            course.TotalLessons++;
            await _uow.Courses.UpdateAsync(course);
            await _uow.SaveChangesAsync(ct);

            return Result<LessonDto>.Success(new LessonDto(
                lesson.Id, lesson.SectionId, lesson.Title, lesson.Description,
                lesson.ContentType.ToString(), lesson.DisplayOrder, lesson.DurationMinutes,
                lesson.IsPreview, lesson.IsDownloadable, lesson.VideoUrl,
                lesson.ArticleContent, lesson.ResourceUrl, false, null, null), 201);
        }
    }
}
