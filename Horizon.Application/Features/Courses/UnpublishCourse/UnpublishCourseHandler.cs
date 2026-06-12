
using Horizon.Application.Common;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Courses.UnpublishCourse
{
    public class UnpublishCourseHandler : IRequestHandler<UnpublishCourseCommand, Result>
    {
        private readonly IUnitOfWork _uow;

        public UnpublishCourseHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(UnpublishCourseCommand request, CancellationToken ct)
        {
            var course = await _uow.Courses.GetByIdAsync(request.CourseId, ct);
            if (course == null) return Result.NotFound("Course not found.");
            if (course.InstructorId != request.InstructorId) return Result.Forbidden();

            if (course.Status == CourseStatus.Draft)
                return Result.Failure("Course is already unpublished.");

            course.Status = CourseStatus.Draft;
            await _uow.Courses.UpdateAsync(course);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
