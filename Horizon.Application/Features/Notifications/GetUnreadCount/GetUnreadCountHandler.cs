

using Horizon.Application.Common;
using Horizon.Domain.Interfaces.Services.NotificationServices;
using MediatR;

namespace Horizon.Application.Features.Notifications.GetUnreadCount
{
    public class GetUnreadCountHandler : IRequestHandler<GetUnreadCountQuery, Result<int>>
    {
        private readonly INotificationService _notifications;
        public GetUnreadCountHandler(INotificationService notifications) => _notifications = notifications;

        public async Task<Result<int>> Handle(GetUnreadCountQuery request, CancellationToken ct)
        {
            var count = await _notifications.GetUnreadCountAsync(request.UserId, ct);
            return Result<int>.Success(count);
        }
    }
}
