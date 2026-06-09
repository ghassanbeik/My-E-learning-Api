

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Courses.RejectCourse
{
    public record RejectCourseCommand(Guid CourseId, string Reason) : IRequest<Result>;

}
