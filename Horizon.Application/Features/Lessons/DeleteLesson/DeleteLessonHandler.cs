
using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Lessons.DeleteLesson
{
    public class DeleteLessonHandler : IRequestHandler<DeleteLessonCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        public DeleteLessonHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(DeleteLessonCommand request, CancellationToken ct)
        {
            var lesson = await _uow.Lessons.GetByIdAsync(request.LessonId, ct);
            if (lesson == null) return Result.NotFound("Lesson not found.");

            var section = await _uow.Sections.GetByIdAsync(lesson.SectionId, ct);
            var course = section != null ? await _uow.Courses.GetByIdAsync(section.CourseId, ct) : null;
            if (course?.InstructorId != request.InstructorId) return Result.Forbidden();

            await _uow.Lessons.DeleteAsync(lesson);
            if (course != null) { course.TotalLessons = Math.Max(0, course.TotalLessons - 1); await _uow.Courses.UpdateAsync(course); }
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
