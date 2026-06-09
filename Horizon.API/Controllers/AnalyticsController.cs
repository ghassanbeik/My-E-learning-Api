using Horizon.API.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.Analytics.GetCourseAnalytics;
using Horizon.Application.Features.Analytics.GetInstructorDashboard;
using Horizon.Application.Features.Analytics.GetPlatformStats;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.API.Controllers
{
    [Route("api/analytics")]
    public class AnalyticsController : BaseController
    {
        private readonly IMediator _mediator;
        public AnalyticsController(IMediator mediator) => _mediator = mediator;

        /// <summary>Get instructor dashboard</summary>
        [HttpGet("instructor/dashboard")]
        [Authorize(Policy = "Instructor")]
        [ProducesResponseType(typeof(ApiResponse<InstructorDashboardDto>), 200)]
        public async Task<IActionResult> InstructorDashboard(CancellationToken ct)
            => FromResult(await _mediator.Send(new GetInstructorDashboardQuery(UserId), ct));

        /// <summary>Get course analytics</summary>
        [HttpGet("courses/{courseId:guid}")]
        [Authorize(Policy = "Instructor")]
        [ProducesResponseType(typeof(ApiResponse<CourseAnalyticsDto>), 200)]
        public async Task<IActionResult> CourseAnalytics(Guid courseId, [FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
            => FromResult(await _mediator.Send(new GetCourseAnalyticsQuery(courseId, UserId, from, to), ct));

        /// <summary>Get platform stats (Admin)</summary>
        [HttpGet("platform")]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<PlatformStatsDto>), 200)]
        public async Task<IActionResult> PlatformStats(CancellationToken ct)
            => FromResult(await _mediator.Send(new GetPlatformStatsQuery(), ct));
    }
}
