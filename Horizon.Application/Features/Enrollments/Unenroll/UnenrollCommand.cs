

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Enrollments.Unenroll
{
    public record UnenrollCommand(Guid StudentId, Guid CourseId) : IRequest<Result>;

}
