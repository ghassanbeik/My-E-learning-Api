namespace Horizon.Domain.Interfaces.Services.PaymentStripeServices
{
    public interface IPaymentService
    {
        Task<PaymentIntentResult> CreatePaymentIntentAsync(
            CreatePaymentIntentRequest request, CancellationToken ct = default);

        Task<PaymentResult> ConfirmPaymentAsync(
            string paymentIntentId, CancellationToken ct = default);

        Task<RefundResult> RefundAsync(
            string transactionId,
            decimal? amount = null,
            string? reason = null,
            CancellationToken ct = default);

        /// <summary>
        /// Validates the Stripe-Signature header, parses the webhook event,
        /// and returns a provider-agnostic result. IsValid = false means the
        /// signature did not match — callers must return HTTP 400.
        /// </summary>
        Task<StripeWebhookResult> ParseWebhookEventAsync(
            string payload, string signature, CancellationToken ct = default);
    }
}
