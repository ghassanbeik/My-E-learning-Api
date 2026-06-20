using Horizon.Application.Common;
using Horizon.Application.Features.Payments.ConfirmPayment;
using Horizon.Domain.Interfaces.Services.PaymentStripeServices;
using MediatR;

namespace Horizon.Application.Features.Payments.HandleWebhook
{
    /// <summary>
    /// Validates the Stripe-Signature header, parses the event type,
    /// and for payment_intent.succeeded events delegates to
    /// ConfirmPaymentHandler — the same handler the client calls via
    /// POST /api/payments/confirm.
    ///
    /// ConfirmPaymentHandler is idempotent so the webhook firing after
    /// the client already confirmed (or vice-versa) produces exactly
    /// one Payment + Enrollment without duplicates.
    /// </summary>
    public class HandleWebhookHandler : IRequestHandler<HandleWebhookCommand, Result<string>>
    {
        private readonly IPaymentService _payment;
        private readonly IMediator _mediator;

        public HandleWebhookHandler(IPaymentService payment, IMediator mediator)
        {
            _payment  = payment;
            _mediator = mediator;
        }

        public async Task<Result<string>> Handle(HandleWebhookCommand request, CancellationToken ct)
        {
            var webhookResult = await _payment.ParseWebhookEventAsync(
                request.Payload, request.StripeSignature, ct);

            if (!webhookResult.IsValid)
                return Result<string>.Failure(webhookResult.Error ?? "Invalid webhook signature.", 400);

            // Only act on successful payment intents — silently acknowledge all
            // other events so Stripe does not retry them.
            if (webhookResult.EventType != "payment_intent.succeeded")
                return Result<string>.Success($"Event '{webhookResult.EventType}' acknowledged.");

            if (webhookResult.PaymentIntentId == null ||
                webhookResult.UserId          == null ||
                webhookResult.CourseId        == null)
            {
                return Result<string>.Failure(
                    "Webhook payload is missing required metadata (userId / courseId).", 400);
            }

            var confirmResult = await _mediator.Send(new ConfirmPaymentCommand(
                webhookResult.UserId.Value,
                webhookResult.CourseId.Value,
                webhookResult.PaymentIntentId), ct);

            return confirmResult.IsSuccess
                ? Result<string>.Success("Payment confirmed and enrollment created.")
                : Result<string>.Failure(
                    $"Webhook processing failed: {confirmResult.Error}", 500);
        }
    }
}
