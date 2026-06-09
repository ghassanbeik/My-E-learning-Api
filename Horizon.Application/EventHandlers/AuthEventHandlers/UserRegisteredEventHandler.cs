

using Horizon.Domain.Events.AuthEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.EmailServices;
using Horizon.Domain.Interfaces.Services.NotificationServices;

namespace Horizon.Application.EventHandlers.AuthEventHandlers
{
    public class UserRegisteredEventHandler : IDomainEventHandler<UserRegisteredEvent>
    {
        private readonly IEmailService _email;
        private readonly INotificationService _notifications;

        public UserRegisteredEventHandler(IEmailService email, INotificationService notifications)
        {
            _email = email;
            _notifications = notifications;
        }

        public async Task HandleAsync(UserRegisteredEvent e, CancellationToken ct = default)
        {
            await _email.SendWelcomeAsync(e.Email, e.FullName, ct);

            await _notifications.SendAsync(new SendNotificationRequest
            {
                RecipientId = e.UserId,
                Title = "Welcome to Horizon!",
                Message = $"Hi {e.FullName}, your account has been created successfully.",
                Type = Domain.Enums.NotificationType.SystemAnnouncement,
                Channel = Domain.Enums.NotificationChannel.InApp,
            }, ct);
        }
    }

    

    
}
