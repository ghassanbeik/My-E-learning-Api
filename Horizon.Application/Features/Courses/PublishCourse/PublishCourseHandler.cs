

using Horizon.Application.Common;
using Horizon.Domain.Enums;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Courses.PublishCourse
{
    public class PublishCourseHandler : IRequestHandler<PublishCourseCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public PublishCourseHandler(IUnitOfWork uow, IEventBus eventBus) { _uow = uow; _eventBus = eventBus; }

        public async Task<Result> Handle(PublishCourseCommand request, CancellationToken ct)
        {
            var course = await _uow.Courses.GetByIdAsync(request.CourseId, ct);
            if (course == null) return Result.NotFound("Course not found.");
            if (course.InstructorId != request.InstructorId) return Result.Forbidden();
            if (course.Status == CourseStatus.Published) return Result.Failure("Course is already published.");

            course.Status = CourseStatus.UnderReview;
           await _uow.Courses.UpdateAsync(course);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
