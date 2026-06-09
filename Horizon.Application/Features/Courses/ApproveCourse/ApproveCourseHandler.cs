

using Horizon.Application.Common;
using Horizon.Domain.Enums;
using Horizon.Domain.Events.CourseEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Courses.ApproveCourse
{
    public class ApproveCourseHandler : IRequestHandler<ApproveCourseCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public ApproveCourseHandler(IUnitOfWork uow, IEventBus eventBus) { _uow = uow; _eventBus = eventBus; }

        public async Task<Result> Handle(ApproveCourseCommand request, CancellationToken ct)
        {
            var course = await _uow.Courses.GetWithDetailsAsync(request.CourseId, ct);
            if (course == null) return Result.NotFound("Course not found.");

            course.Status = CourseStatus.Published;
            await _uow.Courses.UpdateAsync(course);
            await _uow.SaveChangesAsync(ct);

            await _eventBus.PublishAsync(new CourseApprovedEvent
            {
                CourseId = course.Id,
                InstructorId = course.InstructorId,
                CourseTitle = course.Title,
                InstructorEmail = course.Instructor.Email,
                InstructorName = course.Instructor.FullName,
            }, ct);

            return Result.Success();
        }
    }

}
