

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Notifications.UpdateNotificationPreferences
{
    public record UpdateNotificationPreferencesCommand(Guid UserId, List<UpdateNotificationPreferenceDto> Preferences) : IRequest<Result>;

}
