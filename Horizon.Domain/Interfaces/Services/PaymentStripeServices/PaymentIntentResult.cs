namespace Horizon.Domain.Interfaces.Services.PaymentStripeServices
{
    public class PaymentIntentResult
    {
        public bool Success { get; set; }
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public decimal Amount { get; set; }
        public string? Error { get; set; }
    }
}
