namespace Horizon.Domain.Interfaces.Services.PaymentStripeServices
{
    public class CreatePaymentIntentRequest
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string? CouponCode { get; set; }
        public string? CustomerEmail { get; set; }
    }
}
