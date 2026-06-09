namespace Horizon.Domain.Interfaces.Services.PaymentStripeServices
{
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string? TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string? Error { get; set; }
    }
}
