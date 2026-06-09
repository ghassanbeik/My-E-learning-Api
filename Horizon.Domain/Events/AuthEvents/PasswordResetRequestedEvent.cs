

namespace Horizon.Domain.Events.AuthEvents
{
    public class PasswordResetRequestedEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public string Email { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        public string ResetLink { get; init; } = string.Empty;
    }
}
