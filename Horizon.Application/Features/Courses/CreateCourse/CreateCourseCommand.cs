

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Courses.CreateCourse
{
    public record CreateCourseCommand(Guid InstructorId, CreateCourseDto Dto) : IRequest<Result<CourseListDto>>;

}
