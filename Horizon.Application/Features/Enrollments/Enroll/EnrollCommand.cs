

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Enrollments.Enroll
{
    public record EnrollCommand(Guid StudentId, Guid CourseId, string? CouponCode) : IRequest<Result<EnrollmentDto>>;

}
