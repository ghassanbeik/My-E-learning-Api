

namespace Horizon.Application.DTOs
{
    public record CertificateDto(
        Guid Id,
        Guid CourseId,
        string CourseTitle,
        string StudentName,
        string CertificateNumber,
        DateTime IssueDate,
        DateTime? ExpiryDate,
        string? VerificationUrl,
        bool IsRevoked);
}
