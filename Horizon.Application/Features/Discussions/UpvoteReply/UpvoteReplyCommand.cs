
using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Discussions.UpvoteReply
{
    public record UpvoteReplyCommand(Guid ReplyId, Guid UserId) : IRequest<Result>;

}
