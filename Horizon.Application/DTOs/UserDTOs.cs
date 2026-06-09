

namespace Horizon.Application.DTOs
{
    public record UserDto(
       Guid Id,
       string FirstName,
       string LastName,
       string FullName,
       string Email,
       string? AvatarUrl,
       string? Headline,
       bool IsEmailVerified,
       List<string> Roles);

    public record UserProfileDto(
        Guid Id,
        string FirstName,
        string LastName,
        string FullName,
        string Email,
        string? AvatarUrl,
        string? Bio,
        string? Headline,
        string? Website,
        string? Twitter,
        string? LinkedIn,
        string? YouTube,
        bool IsEmailVerified,
        DateTime? LastLoginAt,
        List<string> Roles,
        int TotalEnrollments,
        int TotalCompletedCourses,
        int TotalCertificates);

    public record UpdateProfileDto(
        string? FirstName,
        string? LastName,
        string? Bio,
        string? Headline,
        string? Website,
        string? Twitter,
        string? LinkedIn,
        string? YouTube);

    public record InstructorDto(
        Guid Id,
        string FullName,
        string Email,
        string? AvatarUrl,
        string? Headline,
        string? Bio,
        decimal AverageRating,
        int TotalStudents,
        int TotalCourses,
        decimal TotalEarnings,
        int SubscriberCount,
        bool IsVerified);
}
