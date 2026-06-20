using Horizon.API.Common;
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.Payments.ApproveRefund;
using Horizon.Application.Features.Payments.ConfirmPayment;
using Horizon.Application.Features.Payments.CreatePaymentIntent;
using Horizon.Application.Features.Payments.GetMyPayments;
using Horizon.Application.Features.Payments.HandleWebhook;
using Horizon.Application.Features.Payments.RequestRefund;
using Horizon.Application.Features.Payments.ValidateCoupon;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Horizon.API.Controllers;

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

    /// <summary>Create a Stripe PaymentIntent — returns clientSecret for the frontend SDK.</summary>
    [HttpPost("intent")]
    [Authorize]
    [EnableRateLimiting("auth")]
    [ProducesResponseType(typeof(ApiResponse<PaymentIntentResponseDto>), 200)]
    public async Task<IActionResult> CreateIntent([FromBody] CreatePaymentIntentDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new CreatePaymentIntentCommand(UserId, dto), ct));

    /// <summary>Confirm payment after the Stripe SDK reports success on the client.</summary>
    [HttpPost("confirm")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), 200)]
    public async Task<IActionResult> Confirm([FromBody] ConfirmPaymentDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new ConfirmPaymentCommand(UserId, dto.CourseId, dto.PaymentIntentId), ct));

    /// <summary>Retrieve the authenticated user's payment history.</summary>
    [HttpGet("my")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<PaymentDto>>), 200)]
    public async Task<IActionResult> GetMyPayments(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        => FromResult(await _mediator.Send(new GetMyPaymentsQuery(UserId, page, pageSize), ct));

    /// <summary>Submit a refund request for a completed payment.</summary>
    [HttpPost("refund")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<RefundRequestResponseDto>), 200)]
    public async Task<IActionResult> RequestRefund([FromBody] RefundRequestDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new RequestRefundCommand(UserId, dto), ct));

    /// <summary>Approve a pending refund request (Admin only).</summary>
    [HttpPost("refund/{requestId:guid}/approve")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ApproveRefund(Guid requestId, CancellationToken ct)
        => FromResult(await _mediator.Send(new ApproveRefundCommand(requestId, UserId), ct));

    /// <summary>Validate a coupon code against a course.</summary>
    [HttpPost("validate-coupon")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<ValidateCouponResponseDto>), 200)]
    public async Task<IActionResult> ValidateCoupon([FromBody] ValidateCouponDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new ValidateCouponQuery(UserId, dto), ct));

    /// <summary>
    /// Stripe webhook receiver — validates the Stripe-Signature header and
    /// processes payment_intent.succeeded by creating the Enrollment.
    ///
    /// IMPORTANT: must NOT use [FromBody] — Stripe signature validation
    /// requires the raw, unmodified request body. Binding via [FromBody]
    /// would re-serialize it and break the HMAC check.
    /// </summary>
    [HttpPost("webhook")]
    [AllowAnonymous]
    [EnableRateLimiting("webhook")]
    public async Task<IActionResult> Webhook(CancellationToken ct)
    {
        string payload;
        using (var reader = new StreamReader(HttpContext.Request.Body))
            payload = await reader.ReadToEndAsync(ct);

        var signature = Request.Headers["Stripe-Signature"].ToString();
        if (string.IsNullOrEmpty(signature))
            return BadRequest("Missing Stripe-Signature header.");

        var result = await _mediator.Send(new HandleWebhookCommand(payload, signature), ct);

        // Return 400 only for genuine signature failures so Stripe knows to stop retrying.
        // For unsupported event types (ignored) we return 200 — Stripe expects 200 for ACK.
        if (!result.IsSuccess && result.StatusCode == 400)
            return BadRequest(result.Error);

        return Ok(new { message = result.Value ?? result.Error });
    }
}

public record ConfirmPaymentDto(Guid CourseId, string PaymentIntentId);
