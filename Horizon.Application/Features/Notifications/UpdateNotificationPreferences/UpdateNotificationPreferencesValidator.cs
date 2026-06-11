

using FluentValidation;
using Horizon.Domain.Enums;

namespace Horizon.Application.Features.Notifications.UpdateNotificationPreferences
{
    public class UpdateNotificationPreferencesValidator
       : AbstractValidator<UpdateNotificationPreferencesCommand>
    {
        public UpdateNotificationPreferencesValidator()
        {
            RuleFor(x => x.Preferences).NotEmpty()
                .WithMessage("At least one preference must be provided.");
            RuleForEach(x => x.Preferences).ChildRules(p =>
            {
                p.RuleFor(x => x.NotificationType).NotEmpty()
                    .Must(t => Enum.TryParse<NotificationType>(t, out _))
                    .WithMessage("Invalid notification type.");
            });
        }
    }
    }
