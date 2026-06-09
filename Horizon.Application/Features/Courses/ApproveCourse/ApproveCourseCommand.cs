
using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Courses.ApproveCourse
{
    public record ApproveCourseCommand(Guid CourseId) : IRequest<Result>;

}
