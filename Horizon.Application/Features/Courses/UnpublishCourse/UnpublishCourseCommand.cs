

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Courses.UnpublishCourse
{
    public record UnpublishCourseCommand(Guid CourseId, Guid InstructorId) : IRequest<Result>;

}
