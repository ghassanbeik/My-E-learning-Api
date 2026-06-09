
namespace Horizon.Application.DTOs
{
    public record PayoutDto(
        Guid Id,
        Guid InstructorId,
        string InstructorName,
        decimal Amount,
        string Currency,
        string Status,
        string PayoutMethod,
        DateTime? ProcessedAt,
        DateTime PeriodStart,
        DateTime PeriodEnd,
        int TotalEnrollments,
        decimal TotalRevenue,
        decimal PlatformFee);

    public record RequestPayoutDto(
        string PayoutMethod,
        string PayoutAccount,
        DateTime PeriodStart,
        DateTime PeriodEnd);
}
