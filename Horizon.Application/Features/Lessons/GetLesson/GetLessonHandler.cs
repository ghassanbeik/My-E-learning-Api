
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Lessons.GetLesson
{
    public class GetLessonHandler : IRequestHandler<GetLessonQuery, Result<LessonDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetLessonHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<LessonDto>> Handle(GetLessonQuery request, CancellationToken ct)
        {
            var lesson = await _uow.Lessons.GetWithContentAsync(request.LessonId, ct);
            if (lesson == null) return Result<LessonDto>.NotFound("Lesson not found.");

            var section = await _uow.Sections.GetByIdAsync(lesson.SectionId, ct);
            var isEnrolled = section != null &&
                             await _uow.Enrollments.IsEnrolledAsync(request.UserId, section.CourseId, ct);

            if (!lesson.IsPreview && !isEnrolled)
                return Result<LessonDto>.Forbidden("You must be enrolled to access this lesson.");

            var progress = isEnrolled
                ? (await _uow.Enrollments.GetByStudentAndCourseAsync(request.UserId, section!.CourseId, ct))
                    is { } enrollment
                    ? await _uow.Progresses.GetByEnrollmentAndLessonAsync(enrollment.Id, request.LessonId, ct)
                    : null
                : null;

            return Result<LessonDto>.Success(new LessonDto(
                lesson.Id, lesson.SectionId, lesson.Title, lesson.Description,
                lesson.ContentType.ToString(), lesson.DisplayOrder, lesson.DurationMinutes,
                lesson.IsPreview, lesson.IsDownloadable,
                isEnrolled || lesson.IsPreview ? lesson.VideoUrl : null,
                isEnrolled ? lesson.ArticleContent : null,
                isEnrolled ? lesson.ResourceUrl : null,
                progress?.IsCompleted ?? false,
                progress?.VideoWatchedSeconds,
                progress?.VideoTotalSeconds));
        }
    }
}
