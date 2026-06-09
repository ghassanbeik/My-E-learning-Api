

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Discussions.MarkDiscussionAnswered
{
    public record MarkDiscussionAnsweredCommand(Guid DiscussionId, Guid ReplyId, Guid UserId) : IRequest<Result>;

}
