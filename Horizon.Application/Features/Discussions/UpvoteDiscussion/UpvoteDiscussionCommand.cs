

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Discussions.UpvoteDiscussion
{
    public record UpvoteDiscussionCommand(Guid DiscussionId, Guid UserId) : IRequest<Result>;

}
