namespace Horizon.Domain.Interfaces.Services.EmailServices
{
    public interface IEmailService
    {
        Task SendAsync(EmailMessage message, CancellationToken ct = default);
        Task SendWelcomeAsync(string toEmail, string fullName, CancellationToken ct = default);
        Task SendEmailVerificationAsync(string toEmail, string fullName, string verificationLink, CancellationToken ct = default);
        Task SendPasswordResetAsync(string toEmail, string fullName, string resetLink, CancellationToken ct = default);
        Task SendEnrollmentConfirmationAsync(string toEmail, string fullName, string courseTitle, CancellationToken ct = default);
        Task SendCertificateAsync(string toEmail, string fullName, string courseTitle, string certificateUrl, CancellationToken ct = default);
        Task SendPaymentReceiptAsync(string toEmail, string fullName, PaymentReceiptData receipt, CancellationToken ct = default);
        Task SendRefundConfirmationAsync(string toEmail, string fullName, string courseTitle, decimal amount, CancellationToken ct = default);
        Task SendCourseApprovedAsync(string toEmail, string fullName, string courseTitle, CancellationToken ct = default);
        Task SendCourseRejectedAsync(string toEmail, string fullName, string courseTitle, string reason, CancellationToken ct = default);
        Task SendPayoutNotificationAsync(string toEmail, string fullName, decimal amount, string period, CancellationToken ct = default);
        Task SendLiveSessionReminderAsync(string toEmail, string fullName, string sessionTitle, DateTime scheduledAt, string meetingUrl, CancellationToken ct = default);
        Task SendBulkAsync(IEnumerable<string> toEmails, EmailMessage message, CancellationToken ct = default);
    }
}
