

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Courses.GetCoursesByCategory
{
    public record GetCoursesByCategoryQuery(Guid CategoryId, int Page = 1, int PageSize = 20) : IRequest<Result<PagedResponse<CourseListDto>>>;

}
