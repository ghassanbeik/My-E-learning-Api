

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Notifications.GetNotificationPreferences
{
    public class GetNotificationPreferencesHandler
         : IRequestHandler<GetNotificationPreferencesQuery, Result<List<NotificationPreferenceDto>>>
    {
        private readonly IUnitOfWork _uow;
        public GetNotificationPreferencesHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<List<NotificationPreferenceDto>>> Handle(
            GetNotificationPreferencesQuery request, CancellationToken ct)
        {
            var prefs = await _uow.NotificationPreferences.GetByUserAsync(request.UserId, ct);
            return Result<List<NotificationPreferenceDto>>.Success(prefs.Select(p =>
                new NotificationPreferenceDto(p.Id, p.NotificationType.ToString(),
                    p.InAppEnabled, p.EmailEnabled, p.PushEnabled, p.SmsEnabled)).ToList());
        }
    }
}
