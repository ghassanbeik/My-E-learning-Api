
namespace Horizon.Domain.Events.AuthEvents
{
    public class UserRegisteredEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public string Email { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
    }
}
