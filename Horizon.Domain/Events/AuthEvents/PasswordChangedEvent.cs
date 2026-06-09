

namespace Horizon.Domain.Events.AuthEvents
{
    public class PasswordChangedEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public string Email { get; init; } = string.Empty;
    }
}
