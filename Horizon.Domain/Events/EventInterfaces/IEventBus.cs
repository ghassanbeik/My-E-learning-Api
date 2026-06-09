

namespace Horizon.Domain.Events.EventInterfaces
{
    public interface IEventBus
    {
        Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken ct = default) where TEvent : DomainEvent;
    }
}
