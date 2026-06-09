

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Courses.GetFeaturedCourses
{
    public record GetFeaturedCoursesQuery(int Count = 8) : IRequest<Result<List<CourseListDto>>>;

}
