
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using MediatR;
using System.Net.Mime;

namespace Horizon.Application.Features.Lessons.UpdateLesson
{
    public class UpdateLessonHandler : IRequestHandler<UpdateLessonCommand, Result<LessonDto>>
    {
        private readonly IUnitOfWork _uow;
        public UpdateLessonHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<LessonDto>> Handle(UpdateLessonCommand request, CancellationToken ct)
        {
            var lesson = await _uow.Lessons.GetByIdAsync(request.LessonId, ct);
            if (lesson == null) return Result<LessonDto>.NotFound("Lesson not found.");

            var section = await _uow.Sections.GetByIdAsync(lesson.SectionId, ct);
            var course = section != null ? await _uow.Courses.GetByIdAsync(section.CourseId, ct) : null;
            if (course?.InstructorId != request.InstructorId) return Result<LessonDto>.Forbidden();

            var d = request.Dto;
            if (d.Title != null) lesson.Title = d.Title;
            if (d.Description != null) lesson.Description = d.Description;
            if (d.ContentType != null) lesson.ContentType = Enum.Parse<LessonContentType>(d.ContentType);
            if (d.DisplayOrder != null) lesson.DisplayOrder = d.DisplayOrder.Value;
            if (d.DurationMinutes != null) lesson.DurationMinutes = d.DurationMinutes.Value;
            if (d.IsPreview != null) lesson.IsPreview = d.IsPreview.Value;
            if (d.IsDownloadable != null) lesson.IsDownloadable = d.IsDownloadable.Value;
            if (d.VideoUrl != null) lesson.VideoUrl = d.VideoUrl;
            if (d.ArticleContent != null) lesson.ArticleContent = d.ArticleContent;
            if (d.ResourceUrl != null) lesson.ResourceUrl = d.ResourceUrl;

            await _uow.Lessons.UpdateAsync(lesson);
            await _uow.SaveChangesAsync(ct);

            return Result<LessonDto>.Success(new LessonDto(
                lesson.Id, lesson.SectionId, lesson.Title, lesson.Description,
                lesson.ContentType.ToString(), lesson.DisplayOrder, lesson.DurationMinutes,
                lesson.IsPreview, lesson.IsDownloadable, lesson.VideoUrl,
                lesson.ArticleContent, lesson.ResourceUrl, false, null, null));
        }
    }
}
