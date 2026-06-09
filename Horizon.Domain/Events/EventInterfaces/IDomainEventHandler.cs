

namespace Horizon.Domain.Events.EventInterfaces
{
    public interface IDomainEventHandler<TEvent> where TEvent : DomainEvent
    {
        Task HandleAsync(TEvent domainEvent, CancellationToken ct = default);
    }
}
