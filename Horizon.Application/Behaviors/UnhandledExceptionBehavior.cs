

using Horizon.Application.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Horizon.Application.Behaviors
{
    public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> _logger;

        public UnhandledExceptionBehavior(ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger)
            => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            try
            {
                return await next();
            }
            catch (Exception ex) when (ex is not ValidationException and not NotFoundException
                                           and not UnauthorizedException and not ForbiddenException
                                           and not ConflictException)
            {
                _logger.LogError(ex, "Unhandled exception for request {Request}: {@RequestData}", typeof(TRequest).Name, request);
                throw;
            }
        }
    }
}
