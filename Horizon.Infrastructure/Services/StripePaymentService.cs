
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

        public StripePaymentService(IConfiguration config, ILogger<StripePaymentService> logger, IEventBus eventBus)
        {
            _config = config;
            _logger = logger;
            _eventBus = eventBus;
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }

        public async Task<PaymentIntentResult> CreatePaymentIntentAsync(CreatePaymentIntentRequest request, CancellationToken ct = default)
        {
            try
            {
                var service = new PaymentIntentService();
                var intent = await service.CreateAsync(new PaymentIntentCreateOptions
                {
                    Amount = (long)(request.Amount * 100),
                    Currency = request.Currency.ToLower(),
                    ReceiptEmail = request.CustomerEmail,
                    AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions { Enabled = true },
                    Metadata = new Dictionary<string, string>
                    {
                        ["userId"] = request.UserId.ToString(),
                        ["courseId"] = request.CourseId.ToString(),
                    },
                }, cancellationToken: ct);

                return new PaymentIntentResult
                {
                    Success = true,
                    ClientSecret = intent.ClientSecret,
                    PaymentIntentId = intent.Id,
                    Amount = request.Amount,
                };
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe payment intent creation failed");
                return new PaymentIntentResult { Success = false, Error = ex.Message };
            }
        }

        public async Task<PaymentResult> ConfirmPaymentAsync(string paymentIntentId, CancellationToken ct = default)
        {
            try
            {
                var service = new PaymentIntentService();
                var intent = await service.GetAsync(paymentIntentId, cancellationToken: ct);

                return new PaymentResult
                {
                    Success = intent.Status == "succeeded",
                    TransactionId = intent.Id,
                    Amount = intent.Amount / 100m,
                    Error = intent.Status != "succeeded" ? intent.LastPaymentError?.Message : null,
                };
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe payment confirmation failed for {PaymentIntentId}", paymentIntentId);
                return new PaymentResult { Success = false, Error = ex.Message };
            }
        }

        public async Task<RefundResult> RefundAsync(string transactionId, decimal? amount = null, string? reason = null, CancellationToken ct = default)
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
                        "duplicate" => "duplicate",
                        "fraudulent" => "fraudulent",
                        _ => "requested_by_customer",
                    },
                };

                var refund = await service.CreateAsync(options, cancellationToken: ct);
                return new RefundResult
                {
                    Success = refund.Status == "succeeded",
                    RefundId = refund.Id,
                    Amount = refund.Amount / 100m,
                };
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe refund failed for {TransactionId}", transactionId);
                return new RefundResult { Success = false, Error = ex.Message };
            }
        }

        public Task<bool> ValidateWebhookAsync(string payload, string signature, CancellationToken ct = default)
        {
            try
            {
                EventUtility.ConstructEvent(payload, signature, _config["Stripe:WebhookSecret"]);
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public async Task HandleWebhookAsync(string payload, CancellationToken ct = default)
        {
            // Webhook event routing handled in controller — service just validates
            await Task.CompletedTask;
        }
    }
}
