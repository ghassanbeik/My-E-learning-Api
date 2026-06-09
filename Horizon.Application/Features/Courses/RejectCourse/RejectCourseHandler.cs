

using Horizon.Application.Common;
using Horizon.Domain.Enums;
using Horizon.Domain.Events.CourseEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Courses.RejectCourse
{
    public class RejectCourseHandler : IRequestHandler<RejectCourseCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public RejectCourseHandler(IUnitOfWork uow, IEventBus eventBus) { _uow = uow; _eventBus = eventBus; }

        public async Task<Result> Handle(RejectCourseCommand request, CancellationToken ct)
        {
            var course = await _uow.Courses.GetWithDetailsAsync(request.CourseId, ct);
            if (course == null) return Result.NotFound("Course not found.");

            course.Status = CourseStatus.Rejected;
            await _uow.Courses.UpdateAsync(course);
            await _uow.SaveChangesAsync(ct);

            await _eventBus.PublishAsync(new CourseRejectedEvent
            {
                CourseId = course.Id,
                InstructorId = course.InstructorId,
                CourseTitle = course.Title,
                InstructorEmail = course.Instructor.Email,
                InstructorName = course.Instructor.FullName,
                Reason = request.Reason,
            }, ct);

            return Result.Success();
        }
    }

}
