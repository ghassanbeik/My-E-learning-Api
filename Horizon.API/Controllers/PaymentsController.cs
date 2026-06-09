using Horizon.API.Common;

using Horizon.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Horizon.Application.Features.Payments.CreatePaymentIntent;
using Horizon.Application.Features.Payments.ConfirmPayment;
using Horizon.Application.Common;
using Horizon.Application.Features.Payments.GetMyPayments;
using Horizon.Application.Features.Payments.RequestRefund;
using Horizon.Application.Features.Payments.ApproveRefund;
using Horizon.Application.Features.Payments.ValidateCoupon;

namespace Horizon.API.Controllers;

// ─── Payments ─────────────────────────────────────────────────────────────────

[Route("api/payments")]
public class PaymentsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _config;

    public PaymentsController(IMediator mediator, IConfiguration config)
    {
        _mediator = mediator;
        _config   = config;
    }

    /// <summary>Create Stripe payment intent</summary>
    [HttpPost("intent")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<PaymentIntentResponseDto>), 200)]
    public async Task<IActionResult> CreateIntent([FromBody] CreatePaymentIntentDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new CreatePaymentIntentCommand(UserId, dto), ct));

    /// <summary>Confirm payment after Stripe success</summary>
    [HttpPost("confirm")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), 200)]
    public async Task<IActionResult> Confirm([FromBody] ConfirmPaymentDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new ConfirmPaymentCommand(UserId, dto.CourseId, dto.PaymentIntentId), ct));

    /// <summary>Get my payment history</summary>
    [HttpGet("my")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<PaymentDto>>), 200)]
    public async Task<IActionResult> GetMyPayments([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        => FromResult(await _mediator.Send(new GetMyPaymentsQuery(UserId, page, pageSize), ct));

    /// <summary>Request a refund</summary>
    [HttpPost("refund")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<RefundRequestResponseDto>), 200)]
    public async Task<IActionResult> RequestRefund([FromBody] RefundRequestDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new RequestRefundCommand(UserId, dto), ct));

    /// <summary>Approve refund request (Admin)</summary>
    [HttpPost("refund/{requestId:guid}/approve")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ApproveRefund(Guid requestId, CancellationToken ct)
        => FromResult(await _mediator.Send(new ApproveRefundCommand(requestId, UserId), ct));

    /// <summary>Validate a coupon code</summary>
    [HttpPost("validate-coupon")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<ValidateCouponResponseDto>), 200)]
    public async Task<IActionResult> ValidateCoupon([FromBody] ValidateCouponDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new ValidateCouponQuery(UserId, dto), ct));

    /// <summary>Stripe webhook handler</summary>
    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> Webhook(CancellationToken ct)
    {
        var payload   = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(ct);
        var signature = Request.Headers["Stripe-Signature"].ToString();

        if (string.IsNullOrEmpty(signature))
            return BadRequest(ApiResponse.Failure("Missing Stripe signature."));

        // Webhook validation and processing handled by StripePaymentService
        return Ok();
    }
}

public record ConfirmPaymentDto(Guid CourseId, string PaymentIntentId);
