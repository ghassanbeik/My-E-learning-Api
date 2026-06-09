
namespace Horizon.Domain.Events.AuthEvents
{
    public class UserLoggedInEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public string Email { get; init; } = string.Empty;
        public string? IpAddress { get; init; }
        public string? DeviceInfo { get; init; }
    }
}
