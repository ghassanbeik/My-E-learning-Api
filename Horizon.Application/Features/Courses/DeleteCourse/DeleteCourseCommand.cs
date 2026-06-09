

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Courses.DeleteCourse
{
    public record DeleteCourseCommand(Guid CourseId, Guid InstructorId) : IRequest<Result>;

}
