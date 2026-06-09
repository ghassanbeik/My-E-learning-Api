

using Horizon.Domain.Interfaces.Services.EmailServices;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Horizon.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendAsync(EmailMessage message, CancellationToken ct = default)
        {
            try
            {
                var mime = new MimeMessage();
                mime.From.Add(new MailboxAddress(
                    _config["Email:FromName"],
                    _config["Email:FromAddress"]));
                mime.To.Add(MailboxAddress.Parse(message.To));
                mime.Subject = message.Subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = message.HtmlBody,
                    TextBody = message.PlainTextBody,
                };

                foreach (var attachment in message.Attachments)
                    builder.Attachments.Add(attachment.FileName, attachment.Content,
                        ContentType.Parse(attachment.ContentType));

                mime.Body = builder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _config["Email:SmtpHost"],
                    int.Parse(_config["Email:SmtpPort"] ?? "587"),
                    SecureSocketOptions.StartTls, ct);
                await client.AuthenticateAsync(
                    _config["Email:SmtpUser"],
                    _config["Email:SmtpPassword"], ct);
                await client.SendAsync(mime, ct);
                await client.DisconnectAsync(true, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To} with subject {Subject}", message.To, message.Subject);
            }
        }

        public async Task SendWelcomeAsync(string toEmail, string fullName, CancellationToken ct = default)
            => await SendAsync(new EmailMessage
            {
                To = toEmail,
                Subject = "Welcome to Horizon!",
                HtmlBody = $@"
                <h2>Welcome to Horizon, {fullName}!</h2>
                <p>We're thrilled to have you on board. Start exploring thousands of courses today.</p>
                <a href='https://horizon.com/courses' style='background:#6366f1;color:white;padding:12px 24px;border-radius:6px;text-decoration:none;'>
                    Browse Courses
                </a>",
            }, ct);

        public async Task SendEmailVerificationAsync(string toEmail, string fullName, string verificationLink, CancellationToken ct = default)
            => await SendAsync(new EmailMessage
            {
                To = toEmail,
                Subject = "Verify your email address",
                HtmlBody = $@"
                <h2>Hi {fullName},</h2>
                <p>Please verify your email address by clicking the button below.</p>
                <a href='{verificationLink}' style='background:#6366f1;color:white;padding:12px 24px;border-radius:6px;text-decoration:none;'>
                    Verify Email
                </a>
                <p>This link expires in 24 hours.</p>",
            }, ct);

        public async Task SendPasswordResetAsync(string toEmail, string fullName, string resetLink, CancellationToken ct = default)
            => await SendAsync(new EmailMessage
            {
                To = toEmail,
                Subject = "Reset your password",
                HtmlBody = $@"
                <h2>Hi {fullName},</h2>
                <p>We received a request to reset your password. Click below to proceed.</p>
                <a href='{resetLink}' style='background:#6366f1;color:white;padding:12px 24px;border-radius:6px;text-decoration:none;'>
                    Reset Password
                </a>
                <p>This link expires in 1 hour. If you did not request this, ignore this email.</p>",
            }, ct);

        public async Task SendEnrollmentConfirmationAsync(string toEmail, string fullName, string courseTitle, CancellationToken ct = default)
            => await SendAsync(new EmailMessage
            {
                To = toEmail,
                Subject = $"Enrollment confirmed: {courseTitle}",
                HtmlBody = $@"
                <h2>You're enrolled, {fullName}!</h2>
                <p>You have successfully enrolled in <strong>{courseTitle}</strong>.</p>
                <a href='https://horizon.com/my-courses' style='background:#6366f1;color:white;padding:12px 24px;border-radius:6px;text-decoration:none;'>
                    Start Learning
                </a>",
            }, ct);

        public async Task SendCertificateAsync(string toEmail, string fullName, string courseTitle, string certificateUrl, CancellationToken ct = default)
            => await SendAsync(new EmailMessage
            {
                To = toEmail,
                Subject = $"Your certificate for {courseTitle} is ready!",
                HtmlBody = $@"
                <h2>Congratulations {fullName}! 🎉</h2>
                <p>You have successfully completed <strong>{courseTitle}</strong>.</p>
                <a href='{certificateUrl}' style='background:#6366f1;color:white;padding:12px 24px;border-radius:6px;text-decoration:none;'>
                    Download Certificate
                </a>",
            }, ct);

        public async Task SendPaymentReceiptAsync(string toEmail, string fullName, PaymentReceiptData receipt, CancellationToken ct = default)
            => await SendAsync(new EmailMessage
            {
                To = toEmail,
                Subject = $"Payment receipt - {receipt.CourseTitle}",
                HtmlBody = $@"
                <h2>Payment Receipt</h2>
                <p>Hi {fullName}, thank you for your purchase!</p>
                <table style='width:100%;border-collapse:collapse;'>
                    <tr><td><strong>Course</strong></td><td>{receipt.CourseTitle}</td></tr>
                    <tr><td><strong>Amount</strong></td><td>{receipt.Currency} {receipt.Amount:F2}</td></tr>
                    <tr><td><strong>Transaction ID</strong></td><td>{receipt.TransactionId}</td></tr>
                    <tr><td><strong>Payment Method</strong></td><td>{receipt.PaymentMethod}</td></tr>
                    <tr><td><strong>Date</strong></td><td>{receipt.PaidAt:dddd, MMMM dd yyyy}</td></tr>
                </table>",
            }, ct);

        public async Task SendRefundConfirmationAsync(string toEmail, string fullName, string courseTitle, decimal amount, CancellationToken ct = default)
            => await SendAsync(new EmailMessage
            {
                To = toEmail,
                Subject = "Refund confirmed",
                HtmlBody = $@"
                <h2>Refund Confirmed</h2>
                <p>Hi {fullName}, your refund of <strong>${amount:F2}</strong> for <strong>{courseTitle}</strong> has been processed.</p>
                <p>Please allow 5-10 business days for the amount to appear in your account.</p>",
            }, ct);

        public async Task SendCourseApprovedAsync(string toEmail, string fullName, string courseTitle, CancellationToken ct = default)
            => await SendAsync(new EmailMessage
            {
                To = toEmail,
                Subject = $"Your course '{courseTitle}' has been approved!",
                HtmlBody = $@"
                <h2>Great news, {fullName}!</h2>
                <p>Your course <strong>{courseTitle}</strong> has been approved and is now live on Horizon.</p>
                <a href='https://horizon.com/instructor/courses' style='background:#6366f1;color:white;padding:12px 24px;border-radius:6px;text-decoration:none;'>
                    View Your Course
                </a>",
            }, ct);

        public async Task SendCourseRejectedAsync(string toEmail, string fullName, string courseTitle, string reason, CancellationToken ct = default)
            => await SendAsync(new EmailMessage
            {
                To = toEmail,
                Subject = $"Action required: '{courseTitle}'",
                HtmlBody = $@"
                <h2>Hi {fullName},</h2>
                <p>Your course <strong>{courseTitle}</strong> requires some changes before it can be published.</p>
                <p><strong>Reason:</strong> {reason}</p>
                <a href='https://horizon.com/instructor/courses' style='background:#6366f1;color:white;padding:12px 24px;border-radius:6px;text-decoration:none;'>
                    Edit Course
                </a>",
            }, ct);

        public async Task SendPayoutNotificationAsync(string toEmail, string fullName, decimal amount, string period, CancellationToken ct = default)
            => await SendAsync(new EmailMessage
            {
                To = toEmail,
                Subject = "Your payout has been processed",
                HtmlBody = $@"
                <h2>Payout Processed</h2>
                <p>Hi {fullName}, your payout of <strong>${amount:F2}</strong> for the period <strong>{period}</strong> has been processed.</p>
                <p>Please allow 3-5 business days for the funds to arrive.</p>",
            }, ct);

        public async Task SendLiveSessionReminderAsync(string toEmail, string fullName, string sessionTitle, DateTime scheduledAt, string meetingUrl, CancellationToken ct = default)
            => await SendAsync(new EmailMessage
            {
                To = toEmail,
                Subject = $"Reminder: '{sessionTitle}' starts soon",
                HtmlBody = $@"
                <h2>Hi {fullName},</h2>
                <p>Your live session <strong>{sessionTitle}</strong> starts at {scheduledAt:HH:mm UTC} today.</p>
                <a href='{meetingUrl}' style='background:#6366f1;color:white;padding:12px 24px;border-radius:6px;text-decoration:none;'>
                    Join Session
                </a>",
            }, ct);

        public async Task SendBulkAsync(IEnumerable<string> toEmails, EmailMessage message, CancellationToken ct = default)
        {
            var tasks = toEmails.Select(email =>
                SendAsync(new EmailMessage
                {
                    To = email,
                    Subject = message.Subject,
                    HtmlBody = message.HtmlBody,
                }, ct));
            await Task.WhenAll(tasks);
        }
    }

}
