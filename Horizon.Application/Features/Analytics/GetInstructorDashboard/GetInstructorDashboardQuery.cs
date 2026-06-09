

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Analytics.GetInstructorDashboard
{
    public record GetInstructorDashboardQuery(Guid InstructorId) : IRequest<Result<InstructorDashboardDto>>;
}
