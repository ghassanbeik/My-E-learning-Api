

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Enrollments.GetCourseProgress
{
    public class GetCourseProgressHandler : IRequestHandler<GetCourseProgressQuery, Result<List<ProgressDto>>>
    {
        private readonly IUnitOfWork _uow;

        public GetCourseProgressHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<List<ProgressDto>>> Handle(GetCourseProgressQuery request, CancellationToken ct)
        {
            var enrollment = await _uow.Enrollments.GetByStudentAndCourseAsync(request.StudentId, request.CourseId, ct);
            if (enrollment == null) return Result<List<ProgressDto>>.NotFound("Enrollment not found.");

            var progresses = await _uow.Progresses.GetByEnrollmentAsync(enrollment.Id, ct);

            var items = new List<ProgressDto>();
            foreach (var p in progresses)
            {
                var lesson = await _uow.Lessons.GetByIdAsync(p.LessonId, ct);
                items.Add(new ProgressDto(p.Id, p.LessonId, lesson?.Title ?? string.Empty,
                    p.IsCompleted, p.CompletedAt, p.TimeSpentMinutes,
                    p.VideoWatchedSeconds, p.VideoTotalSeconds));
            }

            return Result<List<ProgressDto>>.Success(items);
        }
    }

}
