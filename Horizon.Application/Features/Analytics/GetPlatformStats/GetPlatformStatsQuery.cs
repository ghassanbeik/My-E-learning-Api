

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Analytics.GetPlatformStats
{
    public record GetPlatformStatsQuery() : IRequest<Result<PlatformStatsDto>>;

}
