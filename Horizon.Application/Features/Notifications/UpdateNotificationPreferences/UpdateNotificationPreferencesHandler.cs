

using Horizon.Application.Common;
using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Notifications.UpdateNotificationPreferences
{
    public class UpdateNotificationPreferencesHandler
        : IRequestHandler<UpdateNotificationPreferencesCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        public UpdateNotificationPreferencesHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(UpdateNotificationPreferencesCommand request, CancellationToken ct)
        {
            foreach (var dto in request.Preferences)
            {
                var type = Enum.Parse<NotificationType>(dto.NotificationType);
                var pref = await _uow.NotificationPreferences
                    .GetByUserAndTypeAsync(request.UserId, type, ct);

                if (pref == null)
                {
                    await _uow.NotificationPreferences.AddAsync(new NotificationPreference
                    {
                        UserId = request.UserId,
                        NotificationType = type,
                        InAppEnabled = dto.InAppEnabled,
                        EmailEnabled = dto.EmailEnabled,
                        PushEnabled = dto.PushEnabled,
                        SmsEnabled = dto.SmsEnabled,
                    }, ct);
                }
                else
                {
                    pref.InAppEnabled = dto.InAppEnabled;
                    pref.EmailEnabled = dto.EmailEnabled;
                    pref.PushEnabled = dto.PushEnabled;
                    pref.SmsEnabled = dto.SmsEnabled;
                    await _uow.NotificationPreferences.UpdateAsync(pref);
                }
            }

            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
