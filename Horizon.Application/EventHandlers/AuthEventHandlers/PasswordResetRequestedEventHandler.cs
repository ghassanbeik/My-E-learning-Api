

using Horizon.Domain.Events.AuthEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces.Services.EmailServices;

namespace Horizon.Application.EventHandlers.AuthEventHandlers
{
    public class PasswordResetRequestedEventHandler : IDomainEventHandler<PasswordResetRequestedEvent>
    {
        private readonly IEmailService _email;

        public PasswordResetRequestedEventHandler(IEmailService email) => _email = email;

        public async Task HandleAsync(PasswordResetRequestedEvent e, CancellationToken ct = default)
            => await _email.SendPasswordResetAsync(e.Email, e.FullName, e.ResetLink, ct);
    }
}
