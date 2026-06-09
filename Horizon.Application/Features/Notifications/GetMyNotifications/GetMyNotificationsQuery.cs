

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Notifications.GetMyNotifications
{
    public record GetMyNotificationsQuery(Guid UserId, int Page = 1, int PageSize = 20) : IRequest<Result<PagedResponse<NotificationDto>>>;
}
