namespace Horizon.Domain.Interfaces.Services.PaymentStripeServices
{
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string? TransactionId { get; set; }
        public decimal Amount { get; set; }

        /// <summary>
        /// Coupon code read back from Stripe PaymentIntent.Metadata so the
        /// confirmation handler never trusts client-supplied input for this.
        /// </summary>
        public string? CouponCode { get; set; }

        public string? Error { get; set; }
    }
}
