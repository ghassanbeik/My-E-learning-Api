

using Horizon.Domain.Events.EnrollmentEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;

namespace Horizon.Application.EventHandlers.EnrollmentEventHandlers
{
    public class LessonCompletedEventHandler : IDomainEventHandler<LessonCompletedEvent>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public LessonCompletedEventHandler(IUnitOfWork uow, IEventBus eventBus)
        {
            _uow = uow;
            _eventBus = eventBus;
        }

        public async Task HandleAsync(LessonCompletedEvent e, CancellationToken ct = default)
        {
            // Update enrollment progress
            await _uow.Enrollments.UpdateProgressAsync(e.EnrollmentId, (decimal)e.ProgressPercentage, ct);
            await _uow.SaveChangesAsync(ct);

            // If 100% complete, fire CourseCompletedEvent
            if (e.ProgressPercentage >= 100)
            {
                var enrollment = await _uow.Enrollments.GetWithProgressAsync(e.EnrollmentId, ct);
                if (enrollment == null) return;

                var student = await _uow.Users.GetByIdAsync(enrollment.StudentId, ct);
                if (student == null) return;

                await _eventBus.PublishAsync(new CourseCompletedEvent
                {
                    EnrollmentId = e.EnrollmentId,
                    StudentId = enrollment.StudentId,
                    CourseId = enrollment.CourseId,
                    StudentEmail = student.Email,
                    StudentName = student.FullName,
                    CourseTitle = enrollment.Course.Title,
                }, ct);
            }
        }
    }
}
