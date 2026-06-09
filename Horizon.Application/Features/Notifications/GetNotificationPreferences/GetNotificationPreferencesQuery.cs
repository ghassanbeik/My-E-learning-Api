

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Notifications.GetNotificationPreferences
{
    public record GetNotificationPreferencesQuery(Guid UserId) : IRequest<Result<List<NotificationPreferenceDto>>>;

}
