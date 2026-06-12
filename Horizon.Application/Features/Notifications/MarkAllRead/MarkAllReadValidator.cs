

using FluentValidation;

namespace Horizon.Application.Features.Notifications.MarkAllRead
{
    public class MarkAllReadValidator : AbstractValidator<MarkAllReadCommand>
    {
        public MarkAllReadValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
