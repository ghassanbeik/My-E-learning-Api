

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Courses.GetCourseById
{
    public record GetCourseByIdQuery(Guid CourseId, Guid? UserId = null) : IRequest<Result<CourseDetailDto>>;

}
