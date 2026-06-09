

using Horizon.Domain.Events.AuthEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.EmailServices;
using Horizon.Domain.Interfaces.Services.NotificationServices;

namespace Horizon.Application.EventHandlers.AuthEventHandlers
{
    public class PasswordChangedEventHandler : IDomainEventHandler<PasswordChangedEvent>
    {
        private readonly IEmailService _email;
        private readonly INotificationService _notifications;
        private readonly IUnitOfWork _uow;

        public PasswordChangedEventHandler(IEmailService email, INotificationService notifications, IUnitOfWork uow)
        {
            _email = email;
            _notifications = notifications;
            _uow = uow;
        }

        public async Task HandleAsync(PasswordChangedEvent e, CancellationToken ct = default)
        {
            var user = await _uow.Users.GetByIdAsync(e.UserId, ct);
            if (user == null) return;

            await _email.SendAsync(new EmailMessage
            {
                To = e.Email,
                Subject = "Your password has been changed",
                HtmlBody = $"<p>Hi {user.FullName},</p><p>Your password was changed successfully. If you did not do this, please contact support immediately.</p>",
            }, ct);

            // Revoke all sessions on password change for security
            await _uow.Sessions.RevokeAllUserSessionsAsync(e.UserId, ct);
            await _uow.SaveChangesAsync(ct);
        }
    }
}
