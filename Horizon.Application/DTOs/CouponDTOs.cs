

namespace Horizon.Application.DTOs
{
    public record CouponDto(
        Guid Id,
        string Code,
        string? Description,
        string Type,
        decimal Value,
        decimal? MaxDiscountAmount,
        decimal? MinOrderAmount,
        DateTime? ExpiryDate,
        int? MaxUses,
        int CurrentUses,
        bool IsActive,
        bool IsValid);

    public record CreateCouponDto(
        string Code,
        string? Description,
        string Type,
        decimal Value,
        decimal? MaxDiscountAmount,
        decimal? MinOrderAmount,
        DateTime? StartDate,
        DateTime? ExpiryDate,
        int? MaxUses,
        int? MaxUsesPerUser,
        List<Guid>? CourseIds,
        List<Guid>? CategoryIds);

    public record ValidateCouponDto(string Code, Guid CourseId);

    public record ValidateCouponResponseDto(
        bool IsValid,
        string? Code,
        string? Type,
        decimal? DiscountValue,
        decimal? MaxDiscountAmount,
        string? Message);
}
