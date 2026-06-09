

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Notifications.GetUnreadCount
{
    public record GetUnreadCountQuery(Guid UserId) : IRequest<Result<int>>;

}
