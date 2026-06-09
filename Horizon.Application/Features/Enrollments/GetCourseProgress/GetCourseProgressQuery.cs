

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Enrollments.GetCourseProgress
{
    public record GetCourseProgressQuery(Guid StudentId, Guid CourseId) : IRequest<Result<List<ProgressDto>>>;

}
