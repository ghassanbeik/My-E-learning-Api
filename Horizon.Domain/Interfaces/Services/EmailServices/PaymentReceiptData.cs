namespace Horizon.Domain.Interfaces.Services.EmailServices
{
    public class PaymentReceiptData
    {
        public string TransactionId { get; set; } = string.Empty;
        public string CourseTitle { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime PaidAt { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
