

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Enrollments.CheckEnrollment
{
    public record CheckEnrollmentQuery(Guid StudentId, Guid CourseId) : IRequest<Result<bool>>;

}
