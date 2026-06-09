
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Courses.GetCourses
{
    public record GetCoursesQuery(CourseSearchDto Search) : IRequest<Result<PagedResponse<CourseListDto>>>;

}
