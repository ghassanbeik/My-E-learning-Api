

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.Enrollments.GetMyEnrollments;
using Horizon.Domain.Entities;
using Horizon.Domain.Events.EnrollmentEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Enrollments.UpdateProgress
{
    public class UpdateProgressHandler : IRequestHandler<UpdateProgressCommand, Result<ProgressDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public UpdateProgressHandler(IUnitOfWork uow, IEventBus eventBus) { _uow = uow; _eventBus = eventBus; }

        public async Task<Result<ProgressDto>> Handle(UpdateProgressCommand request, CancellationToken ct)
        {
            var enrollment = await _uow.Enrollments.GetByStudentAndCourseAsync(request.StudentId, request.CourseId, ct);
            if (enrollment == null) return Result<ProgressDto>.NotFound("Enrollment not found.");

            var lesson = await _uow.Lessons.GetByIdAsync(request.LessonId, ct);
            if (lesson == null) return Result<ProgressDto>.NotFound("Lesson not found.");

            var progress = await _uow.Progresses.GetByEnrollmentAndLessonAsync(enrollment.Id, request.LessonId, ct);

            if (progress == null)
            {
                progress = new Progress
                {
                    EnrollmentId = enrollment.Id,
                    LessonId = request.LessonId,
                    IsCompleted = request.Dto.IsCompleted,
                    CompletedAt = request.Dto.IsCompleted ? DateTime.UtcNow : null,
                    VideoWatchedSeconds = request.Dto.VideoWatchedSeconds,
                    VideoTotalSeconds = request.Dto.VideoTotalSeconds,
                    TimeSpentMinutes = request.Dto.TimeSpentMinutes ?? 0,
                    LastAccessedAt = DateTime.UtcNow,
                    AttemptCount = 1,
                };
                await _uow.Progresses.AddAsync(progress, ct);
            }
            else
            {
                if (request.Dto.IsCompleted && !progress.IsCompleted)
                {
                    progress.IsCompleted = true;
                    progress.CompletedAt = DateTime.UtcNow;
                }
                if (request.Dto.VideoWatchedSeconds.HasValue)
                    progress.VideoWatchedSeconds = request.Dto.VideoWatchedSeconds;
                if (request.Dto.VideoTotalSeconds.HasValue)
                    progress.VideoTotalSeconds = request.Dto.VideoTotalSeconds;
                if (request.Dto.TimeSpentMinutes.HasValue)
                    progress.TimeSpentMinutes += request.Dto.TimeSpentMinutes.Value;
                progress.LastAccessedAt = DateTime.UtcNow;
                progress.AttemptCount++;
            }

            await _uow.SaveChangesAsync(ct);

            var percentage = await _uow.Progresses.GetCompletionPercentageAsync(enrollment.Id, ct);

            await _eventBus.PublishAsync(new LessonCompletedEvent
            {
                EnrollmentId = enrollment.Id,
                StudentId = request.StudentId,
                LessonId = request.LessonId,
                CourseId = request.CourseId,
                ProgressPercentage = percentage,
            }, ct);

            return Result<ProgressDto>.Success(new ProgressDto(
                progress.Id, progress.LessonId, lesson.Title, progress.IsCompleted,
                progress.CompletedAt, progress.TimeSpentMinutes,
                progress.VideoWatchedSeconds, progress.VideoTotalSeconds));
        }
    }

   
}
