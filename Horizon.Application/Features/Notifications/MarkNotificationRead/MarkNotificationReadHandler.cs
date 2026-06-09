

using Horizon.Application.Common;
using Horizon.Domain.Interfaces.Services.NotificationServices;
using MediatR;

namespace Horizon.Application.Features.Notifications.MarkNotificationRead
{
    public class MarkNotificationReadHandler : IRequestHandler<MarkNotificationReadCommand, Result>
    {
        private readonly INotificationService _notifications;
        public MarkNotificationReadHandler(INotificationService notifications) => _notifications = notifications;

        public async Task<Result> Handle(MarkNotificationReadCommand request, CancellationToken ct)
        {
            await _notifications.MarkReadAsync(request.NotificationId, ct);
            return Result.Success();
        }
    }
}
