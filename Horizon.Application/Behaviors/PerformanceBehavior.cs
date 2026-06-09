

using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Horizon.Application.Behaviors
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
        private const int ThresholdMs = 500;

        public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
            => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            var sw = Stopwatch.StartNew();
            var response = await next();
            sw.Stop();

            if (sw.ElapsedMilliseconds > ThresholdMs)
                _logger.LogWarning("Slow request: {Request} took {Elapsed}ms", typeof(TRequest).Name, sw.ElapsedMilliseconds);

            return response;
        }
    }
}
