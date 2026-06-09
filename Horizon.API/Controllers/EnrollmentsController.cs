using Horizon.API.Common;
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.Enrollments.CheckEnrollment;
using Horizon.Application.Features.Enrollments.Enroll;
using Horizon.Application.Features.Enrollments.GetCourseProgress;
using Horizon.Application.Features.Enrollments.GetEnrollmentDetail;
using Horizon.Application.Features.Enrollments.GetMyEnrollments;
using Horizon.Application.Features.Enrollments.UpdateProgress;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.API.Controllers;

// ─── Enrollments ──────────────────────────────────────────────────────────────

[Route("api/enrollments")]
public class EnrollmentsController : BaseController
{
    private readonly IMediator _mediator;
    public EnrollmentsController(IMediator mediator) => _mediator = mediator;

    /// <summary>Get my enrollments</summary>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<EnrollmentDto>>), 200)]
    public async Task<IActionResult> GetMyEnrollments([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        => FromResult(await _mediator.Send(new GetMyEnrollmentsQuery(UserId, page, pageSize), ct));

    /// <summary>Get enrollment detail</summary>
    [HttpGet("{enrollmentId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<EnrollmentDetailDto>), 200)]
    public async Task<IActionResult> GetDetail(Guid enrollmentId, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetEnrollmentDetailQuery(enrollmentId, UserId), ct));

    /// <summary>Enroll in a course</summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<EnrollmentDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse), 409)]
    public async Task<IActionResult> Enroll([FromBody] EnrollDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new EnrollCommand(UserId, dto.CourseId, dto.CouponCode), ct));

    /// <summary>Check if enrolled in a course</summary>
    [HttpGet("check/{courseId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    public async Task<IActionResult> CheckEnrollment(Guid courseId, CancellationToken ct)
        => FromResult(await _mediator.Send(new CheckEnrollmentQuery(UserId, courseId), ct));

    /// <summary>Get course progress</summary>
    [HttpGet("{courseId:guid}/progress")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<List<ProgressDto>>), 200)]
    public async Task<IActionResult> GetProgress(Guid courseId, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetCourseProgressQuery(UserId, courseId), ct));

    /// <summary>Update lesson progress</summary>
    [HttpPut("{courseId:guid}/progress/{lessonId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<ProgressDto>), 200)]
    public async Task<IActionResult> UpdateProgress(Guid courseId, Guid lessonId, [FromBody] UpdateProgressDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new UpdateProgressCommand(UserId, courseId, lessonId, dto), ct));
}

public record EnrollDto(Guid CourseId, string? CouponCode);

// ─── Reviews ──────────────────────────────────────────────────────────────────




// ─── Lesson Notes & Bookmarks ─────────────────────────────────────────────────

