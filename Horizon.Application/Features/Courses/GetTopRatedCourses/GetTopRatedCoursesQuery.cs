

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Courses.GetTopRatedCourses
{
    public record GetTopRatedCoursesQuery(int Count = 8) : IRequest<Result<List<CourseListDto>>>;

}
