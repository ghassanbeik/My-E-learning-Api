

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Enrollments.GetMyEnrollments
{
    public record GetMyEnrollmentsQuery(Guid StudentId, int Page = 1, int PageSize = 20) : IRequest<Result<PagedResponse<EnrollmentDto>>>;

}
