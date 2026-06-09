

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Notifications.MarkNotificationRead
{
    public record MarkNotificationReadCommand(Guid NotificationId, Guid UserId) : IRequest<Result>;

}
