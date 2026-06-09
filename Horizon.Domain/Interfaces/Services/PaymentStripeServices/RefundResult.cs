namespace Horizon.Domain.Interfaces.Services.PaymentStripeServices
{
    public class RefundResult
    {
        public bool Success { get; set; }
        public string? RefundId { get; set; }
        public decimal Amount { get; set; }
        public string? Error { get; set; }
    }
}
