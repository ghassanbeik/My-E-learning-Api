
using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Courses.PublishCourse
{
    public record PublishCourseCommand(Guid CourseId, Guid InstructorId) : IRequest<Result>;

}
