

using Horizon.Domain.Events;
using Horizon.Domain.Events.EventInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Horizon.Infrastructure.Services
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<InMemoryEventBus> _logger;

        public InMemoryEventBus(IServiceProvider services, ILogger<InMemoryEventBus> logger)
        {
            _services = services;
            _logger = logger;
        }

        public async Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken ct = default) where TEvent : DomainEvent
        {
            var handlerType = typeof(IDomainEventHandler<TEvent>);
            var handlers = _services.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                try
                {
                    await ((IDomainEventHandler<TEvent>)handler!).HandleAsync(domainEvent, ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Event handler {Handler} failed for event {Event}",
                        handler!.GetType().Name, typeof(TEvent).Name);
                }
            }
        }
    }
}
