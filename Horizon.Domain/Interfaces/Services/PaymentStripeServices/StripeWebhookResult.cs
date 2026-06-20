namespace Horizon.Domain.Interfaces.Services.PaymentStripeServices
{
    /// <summary>
    /// Normalized result of parsing a Stripe webhook event.
    /// Keeps Stripe SDK types completely out of the Application layer.
    /// </summary>
    public class StripeWebhookResult
    {
        public bool IsValid { get; set; }

        /// <summary>e.g. "payment_intent.succeeded"</summary>
        public string EventType { get; set; } = string.Empty;

        public string? PaymentIntentId { get; set; }

        /// <summary>Parsed from PaymentIntent.Metadata["userId"].</summary>
        public Guid? UserId { get; set; }

        /// <summary>Parsed from PaymentIntent.Metadata["courseId"].</summary>
        public Guid? CourseId { get; set; }

        /// <summary>Parsed from PaymentIntent.Metadata["couponCode"].</summary>
        public string? CouponCode { get; set; }

        public string? Error { get; set; }
    }
}
