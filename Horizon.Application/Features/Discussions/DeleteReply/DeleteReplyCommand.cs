

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Discussions.DeleteReply
{
    public record DeleteReplyCommand(Guid ReplyId, Guid UserId) : IRequest<Result>;

}
