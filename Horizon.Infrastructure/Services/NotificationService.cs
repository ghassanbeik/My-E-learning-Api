

using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.NotificationServices;
using Microsoft.Extensions.Logging;

namespace Horizon.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IUnitOfWork uow, ILogger<NotificationService> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task SendAsync(SendNotificationRequest request, CancellationToken ct = default)
        {
            try
            {
                // Check user preferences
                var pref = await _uow.NotificationPreferences
                    .GetByUserAndTypeAsync(request.RecipientId, request.Type, ct);

                if (pref != null && request.Channel == NotificationChannel.InApp && !pref.InAppEnabled) return;
                if (pref?.MutedUntil.HasValue == true && pref.MutedUntil > DateTime.UtcNow) return;

                var notification = new Notification
                {
                    RecipientId = request.RecipientId,
                    Title = request.Title,
                    Message = request.Message,
                    Type = request.Type,
                    Channel = request.Channel,
                    Status = NotificationStatus.Unread,
                    ActionUrl = request.ActionUrl,
                    ImageUrl = request.ImageUrl,
                    RelatedEntityId = request.RelatedEntityId,
                    RelatedEntityType = request.RelatedEntityType,
                    SenderName = request.SenderName,
                    SenderId = request.SenderId,
                    SentAt = DateTime.UtcNow,
                };

                await _uow.Notifications.AddAsync(notification, ct);
                await _uow.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send notification to {RecipientId}", request.RecipientId);
            }
        }

        public async Task SendToManyAsync(IEnumerable<Guid> recipientIds, SendNotificationRequest request, CancellationToken ct = default)
        {
            var tasks = recipientIds.Select(id => SendAsync(new SendNotificationRequest
            {
                RecipientId = id,
                Title = request.Title,
                Message = request.Message,
                Type = request.Type,
                Channel = request.Channel,
                ActionUrl = request.ActionUrl,
                ImageUrl = request.ImageUrl,
                RelatedEntityId = request.RelatedEntityId,
                RelatedEntityType = request.RelatedEntityType,
                SenderName = request.SenderName,
                SenderId = request.SenderId,
            }, ct));
            await Task.WhenAll(tasks);
        }

        public async Task SendToRoleAsync(string role, SendNotificationRequest request, CancellationToken ct = default)
        {
            var users = await _uow.Users.GetInstructorsAsync(ct); // extend for other roles as needed
            await SendToManyAsync(users.Select(u => u.Id), request, ct);
        }

        public async Task MarkReadAsync(Guid notificationId, CancellationToken ct = default)
        {
            await _uow.Notifications.MarkAsReadAsync(notificationId, ct);
            await _uow.SaveChangesAsync(ct);
        }

        public async Task MarkAllReadAsync(Guid userId, CancellationToken ct = default)
        {
            await _uow.Notifications.MarkAllAsReadAsync(userId, ct);
            await _uow.SaveChangesAsync(ct);
        }

        public async Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default)
            => await _uow.Notifications.GetUnreadCountAsync(userId, ct);
    }
}
