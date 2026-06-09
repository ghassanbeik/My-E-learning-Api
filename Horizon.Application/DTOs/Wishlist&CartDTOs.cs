

namespace Horizon.Application.DTOs
{
    public record WishlistDto(
         Guid Id,
         Guid CourseId,
         string CourseTitle,
         string? CourseThumbnail,
         string InstructorName,
         decimal CurrentPrice,
         double AverageRating,
         DateTime AddedAt);

    public record CartItemDto(
        Guid Id,
        Guid CourseId,
        string CourseTitle,
        string? CourseThumbnail,
        string InstructorName,
        decimal OriginalPrice,
        decimal? DiscountPrice,
        decimal CurrentPrice,
        string? CouponCode,
        decimal? DiscountAmount,
        DateTime AddedAt);

    public record CartSummaryDto(
        List<CartItemDto> Items,
        decimal SubTotal,
        decimal DiscountAmount,
        decimal Total,
        string Currency);
}
