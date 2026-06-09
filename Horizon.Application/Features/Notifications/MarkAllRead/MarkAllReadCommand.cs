
using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Notifications.MarkAllRead
{
    public record MarkAllReadCommand(Guid UserId) : IRequest<Result>;

}
