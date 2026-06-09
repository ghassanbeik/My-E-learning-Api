

namespace Horizon.Domain.Events.AuthEvents
{
    public class UserEmailVerifiedEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public string Email { get; init; } = string.Empty;
    }
}
