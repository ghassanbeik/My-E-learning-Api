

using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.CacheServices;
using MediatR;

namespace Horizon.Application.Features.Courses.DeleteCourse
{
    public class DeleteCourseHandler : IRequestHandler<DeleteCourseCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cache;

        public DeleteCourseHandler(IUnitOfWork uow, ICacheService cache) { _uow = uow; _cache = cache; }

        public async Task<Result> Handle(DeleteCourseCommand request, CancellationToken ct)
        {
            var course = await _uow.Courses.GetByIdAsync(request.CourseId, ct);
            if (course == null) return Result.NotFound("Course not found.");
            if (course.InstructorId != request.InstructorId) return Result.Forbidden();
            if (course.TotalStudents > 0)
                return Result.Failure("Cannot delete a course with enrolled students.");

            await _uow.Courses.DeleteAsync(course, ct);
            await _uow.SaveChangesAsync(ct);
            await _cache.RemoveAsync(CacheKeys.Course(course.Id), ct);
            return Result.Success();
        }
    }
}
