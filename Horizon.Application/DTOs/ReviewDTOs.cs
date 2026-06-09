

namespace Horizon.Application.DTOs
{
    public record ReviewDto(
        Guid Id,
        Guid CourseId,
        string CourseTitle,
        Guid StudentId,
        string StudentName,
        string? StudentAvatar,
        int Rating,
        string? Comment,
        string Status,
        int HelpfulCount,
        List<ReviewResponseDto> Responses,
        DateTime CreatedAt);

    public record CreateReviewDto(Guid CourseId, int Rating, string? Comment);
    public record UpdateReviewDto(int? Rating, string? Comment);

    public record ReviewResponseDto(
        Guid Id,
        Guid ResponderId,
        string ResponderName,
        string? ResponderAvatar,
        string Response,
        DateTime CreatedAt);

    public record CreateReviewResponseDto(string Response);
}
