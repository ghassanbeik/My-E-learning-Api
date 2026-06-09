namespace Horizon.Domain.Interfaces.Services.PaymentStripeServices
{
    public interface IPaymentService
    {
        Task<PaymentIntentResult> CreatePaymentIntentAsync(CreatePaymentIntentRequest request, CancellationToken ct = default);
        Task<PaymentResult> ConfirmPaymentAsync(string paymentIntentId, CancellationToken ct = default);
        Task<RefundResult> RefundAsync(string transactionId, decimal? amount = null, string? reason = null, CancellationToken ct = default);
        Task<bool> ValidateWebhookAsync(string payload, string signature, CancellationToken ct = default);
        Task HandleWebhookAsync(string payload, CancellationToken ct = default);
    }
}
