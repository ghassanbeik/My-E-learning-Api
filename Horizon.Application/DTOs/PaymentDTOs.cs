

namespace Horizon.Application.DTOs
{
    public record PaymentDto(
         Guid Id,
         Guid? EnrollmentId,
         string TransactionId,
         string PaymentMethod,
         decimal Amount,
         string Currency,
         string Status,
         DateTime? PaidAt,
         decimal? RefundAmount,
         string? ReceiptUrl);

    public record CreatePaymentIntentDto(Guid CourseId, string? CouponCode);

    public record PaymentIntentResponseDto(
        string ClientSecret,
        string PaymentIntentId,
        decimal Amount,
        string Currency);

    public record RefundRequestDto(Guid PaymentId, string Reason);

    public record RefundRequestResponseDto(
        Guid Id,
        string Status,
        string Reason,
        DateTime CreatedAt);
}
