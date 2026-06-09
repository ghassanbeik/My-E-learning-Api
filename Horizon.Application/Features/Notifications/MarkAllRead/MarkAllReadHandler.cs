

using Horizon.Application.Common;
using Horizon.Domain.Interfaces.Services.NotificationServices;
using MediatR;

namespace Horizon.Application.Features.Notifications.MarkAllRead
{
    public class MarkAllReadHandler : IRequestHandler<MarkAllReadCommand, Result>
    {
        private readonly INotificationService _notifications;
        public MarkAllReadHandler(INotificationService notifications) => _notifications = notifications;

        public async Task<Result> Handle(MarkAllReadCommand request, CancellationToken ct)
        {
            await _notifications.MarkAllReadAsync(request.UserId, ct);
            return Result.Success();
        }
    }
}
