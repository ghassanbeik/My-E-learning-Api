

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Courses.UpdateCourse
{
    public record UpdateCourseCommand(Guid CourseId, Guid InstructorId, UpdateCourseDto Dto) : IRequest<Result<CourseListDto>>;

}
