using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces.Services.PaymentStripeServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;

namespace Horizon.Infrastructure.Services
{
    public class StripePaymentService : IPaymentService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<StripePaymentService> _logger;
        private readonly IEventBus _eventBus;

        public StripePaymentService(
            IConfiguration config,
            ILogger<StripePaymentService> logger,
            IEventBus eventBus)
        {
            _config   = config;
            _logger   = logger;
            _eventBus = eventBus;
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }

        // ── Create intent ─────────────────────────────────────────────────────

        public async Task<PaymentIntentResult> CreatePaymentIntentAsync(
            CreatePaymentIntentRequest request, CancellationToken ct = default)
        {
            try
            {
                var metadata = new Dictionary<string, string>
                {
                    ["userId"]   = request.UserId.ToString(),
                    ["courseId"] = request.CourseId.ToString(),
                };
                if (!string.IsNullOrEmpty(request.CouponCode))
                    metadata["couponCode"] = request.CouponCode;

                // Idempotency key: same user + course + coupon always maps to
                // the same PaymentIntent so retried taps don't create duplicates.
                var idempotencyKey =
                    $"pi_{request.UserId}_{request.CourseId}_{request.CouponCode ?? "none"}";

                var service = new PaymentIntentService();
                var intent  = await service.CreateAsync(new PaymentIntentCreateOptions
                {
                    Amount      = (long)(request.Amount * 100),
                    Currency    = request.Currency.ToLower(),
                    ReceiptEmail = request.CustomerEmail,
                    AutomaticPaymentMethods =
                        new PaymentIntentAutomaticPaymentMethodsOptions { Enabled = true },
                    Metadata = metadata,
                }, new RequestOptions { IdempotencyKey = idempotencyKey }, cancellationToken: ct);

                return new PaymentIntentResult
                {
                    Success         = true,
                    ClientSecret    = intent.ClientSecret,
                    PaymentIntentId = intent.Id,
                    Amount          = request.Amount,
                };
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe payment intent creation failed");
                return new PaymentIntentResult { Success = false, Error = ex.Message };
            }
        }

        // ── Confirm ───────────────────────────────────────────────────────────

        public async Task<PaymentResult> ConfirmPaymentAsync(
            string paymentIntentId, CancellationToken ct = default)
        {
            try
            {
                var service = new PaymentIntentService();
                var intent  = await service.GetAsync(paymentIntentId, cancellationToken: ct);

                intent.Metadata.TryGetValue("couponCode", out var couponCode);

                return new PaymentResult
                {
                    Success       = intent.Status == "succeeded",
                    TransactionId = intent.Id,
                    Amount        = intent.Amount / 100m,
                    CouponCode    = couponCode,
                    Error         = intent.Status != "succeeded"
                                        ? intent.LastPaymentError?.Message
                                        : null,
                };
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex,
                    "Stripe payment confirmation failed for {PaymentIntentId}", paymentIntentId);
                return new PaymentResult { Success = false, Error = ex.Message };
            }
        }

        // ── Refund ────────────────────────────────────────────────────────────

        public async Task<RefundResult> RefundAsync(
            string transactionId,
            decimal? amount = null,
            string? reason = null,
            CancellationToken ct = default)
        {
            try
            {
                var service = new RefundService();
                var options = new RefundCreateOptions
                {
                    PaymentIntent = transactionId,
                    Amount = amount.HasValue ? (long?)(amount.Value * 100) : null,
                    Reason = reason switch
                    {
                        "duplicate"   => "duplicate",
                        "fraudulent"  => "fraudulent",
                        _             => "requested_by_customer",
                    },
                };

                var refund = await service.CreateAsync(options, cancellationToken: ct);
                return new RefundResult
                {
                    Success  = refund.Status == "succeeded",
                    RefundId = refund.Id,
                    Amount   = refund.Amount / 100m,
                };
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe refund failed for {TransactionId}", transactionId);
                return new RefundResult { Success = false, Error = ex.Message };
            }
        }

        // ── Webhook ───────────────────────────────────────────────────────────

        public Task<StripeWebhookResult> ParseWebhookEventAsync(
            string payload, string signature, CancellationToken ct = default)
        {
            Event stripeEvent;
            try
            {
                // ConstructEvent verifies the HMAC signature — this is the
                // only thing that makes a webhook request trustworthy.
                stripeEvent = EventUtility.ConstructEvent(
                    payload, signature, _config["Stripe:WebhookSecret"]);
            }
            catch (StripeException ex)
            {
                _logger.LogWarning(ex, "Stripe webhook signature validation failed.");
                return Task.FromResult(new StripeWebhookResult
                {
                    IsValid = false,
                    Error   = "Invalid webhook signature.",
                });
            }

            var result = new StripeWebhookResult
            {
                IsValid   = true,
                EventType = stripeEvent.Type,
            };

            if (stripeEvent.Data.Object is PaymentIntent intent)
            {
                result.PaymentIntentId = intent.Id;

                if (intent.Metadata.TryGetValue("userId", out var userIdRaw)
                    && Guid.TryParse(userIdRaw, out var userId))
                    result.UserId = userId;

                if (intent.Metadata.TryGetValue("courseId", out var courseIdRaw)
                    && Guid.TryParse(courseIdRaw, out var courseId))
                    result.CourseId = courseId;

                if (intent.Metadata.TryGetValue("couponCode", out var couponCode))
                    result.CouponCode = couponCode;
            }

            return Task.FromResult(result);
        }
    }
}
