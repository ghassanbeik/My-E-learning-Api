

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Courses.GetInstructorCourses
{
    public record GetInstructorCoursesQuery(Guid InstructorId) : IRequest<Result<List<CourseListDto>>>;

}
